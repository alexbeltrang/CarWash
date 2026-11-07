using CarWash.DTOs;
using CarWash.Utilidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CarWash.Controladores
{
    public sealed class RetiroVehiculosController
    {
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }) { Timeout = TimeSpan.FromSeconds(30) };
        private readonly string baseUrl = (ConfigurationManager.AppSettings["ApiBaseUrl"] ?? string.Empty).TrimEnd('/');

        public Task<ComprobanteRetiroDTO> EmitirAsync(string placa, CancellationToken cancellationToken) =>
            ReceiptAsync(HttpMethod.Post, "api/RetiroVehiculos/comprobante", new { placa = placa.Trim().ToUpperInvariant() }, cancellationToken);

        public Task<ComprobanteRetiroDTO> ConsultarAsync(int idTurno, CancellationToken cancellationToken) =>
            ReceiptAsync(HttpMethod.Get, "api/RetiroVehiculos/" + idTurno, null, cancellationToken);

        public Task<ComprobanteRetiroDTO> EntregarAsync(int idTurno, CancellationToken cancellationToken) =>
            ReceiptAsync(HttpMethod.Post, "api/RetiroVehiculos/" + idTurno + "/entrega", new { revisionConfirmada = true }, cancellationToken);

        public async Task<string> CorreoAsync(int idTurno, CancellationToken cancellationToken)
        {
            var result = await SendAsync(HttpMethod.Post, "api/Turnos/" + idTurno + "/notificacion-email", null, cancellationToken);
            return result.Value<string>("estado") ?? "UNKNOWN";
        }

        public async Task<ImpresionComprobanteDTO> ImprimirAsync(int idTurno, Guid solicitudId, int anchoMm, CancellationToken cancellationToken)
        {
            var job = ParsePrint(await SendAsync(HttpMethod.Post, "api/RetiroVehiculos/" + idTurno + "/impresiones", new { solicitudId, anchoMm }, cancellationToken));
            if (job == null || job.IdTurno != idTurno || job.SolicitudId != solicitudId || job.AnchoMm != anchoMm) throw new IOException("Respuesta de impresion no valida. Consulte el estado.");
            return job;
        }

        public async Task<ImpresionComprobanteDTO?> ConsultarImpresionAsync(Guid solicitudId, CancellationToken cancellationToken)
        {
            var job = ParsePrint(await SendAsync(HttpMethod.Get, "api/RetiroVehiculos/impresiones/" + solicitudId, null, cancellationToken));
            if (job != null && job.SolicitudId != solicitudId) throw new IOException("Respuesta de impresion no valida.");
            return job;
        }

        public async Task<ImpresionComprobanteDTO> ImprimirIngresoAsync(int idTurno, int[] idServicios, CancellationToken cancellationToken)
        {
            var job = ParsePrint(await SendAsync(HttpMethod.Post, "api/RetiroVehiculos/" + idTurno + "/impresiones/ingreso", new { idServicios }, cancellationToken));
            if (job == null || job.IdTurno != idTurno || job.AnchoMm != 80) throw new IOException("Respuesta de impresion no valida. Consulte el estado.");
            return job;
        }

        public async Task<ImpresionComprobanteDTO?> UltimaImpresionAsync(int idTurno, CancellationToken cancellationToken)
        {
            var job = ParsePrint(await SendAsync(HttpMethod.Get, "api/RetiroVehiculos/" + idTurno + "/impresiones/ultima", null, cancellationToken));
            if (job != null && job.IdTurno != idTurno) throw new IOException("Respuesta de impresion no valida.");
            return job;
        }

        private static ImpresionComprobanteDTO? ParsePrint(JObject body)
        {
            if (body.Count == 0) return null;
            var job = body.ToObject<ImpresionComprobanteDTO>();
            if (job == null || job.SolicitudId == Guid.Empty || job.IdTurno <= 0 || (job.AnchoMm != 58 && job.AnchoMm != 80)
                || string.IsNullOrWhiteSpace(job.Impresora) || Array.IndexOf(new[] { "PENDING", "SUBMITTING", "SUBMITTED", "FAILED", "UNKNOWN" }, job.Estado) < 0)
                throw new IOException("Estado de impresion no valido. Consulte nuevamente.");
            return job;
        }

        private async Task<ComprobanteRetiroDTO> ReceiptAsync(HttpMethod method, string path, object? body, CancellationToken cancellationToken)
        {
            var receipt = (await SendAsync(method, path, body, cancellationToken)).ToObject<ComprobanteRetiroDTO>();
            if (receipt == null || receipt.IdTurno <= 0 || !Regex.IsMatch(receipt.Codigo ?? "", "\\A[0-9A-F]{16}\\z")
                || string.IsNullOrWhiteSpace(receipt.Placa) || receipt.Servicios == null || receipt.Servicios.Count == 0)
                throw new IOException("El comprobante recibido no es valido.");
            return receipt;
        }

        private async Task<JObject> SendAsync(HttpMethod method, string path, object? body, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(method, baseUrl + "/" + path))
            {
                clsConnection.AddSessionCredential(request);
                if (body != null) request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                using (var response = await Client.SendAsync(request, cancellationToken))
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        clsConnection.SetApiSession(null, baseUrl, false);
                        throw new UnauthorizedAccessException("La sesion vencio. Inicie sesion nuevamente.");
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        if (method == HttpMethod.Get && path.Contains("/impresiones/") && response.StatusCode == HttpStatusCode.NotFound) return new JObject();
                        if (path.Contains("/impresiones")) throw new IOException(response.StatusCode == HttpStatusCode.ServiceUnavailable
                            ? "Impresion no disponible en el servidor." : "No se pudo completar la solicitud de impresion. Consulte su estado.");
                        var message = response.StatusCode == HttpStatusCode.NotFound ? "No se encontro el ingreso o el comprobante."
                            : response.StatusCode == HttpStatusCode.Conflict ? "Retiro ya registrado o servicios incompletos. Consulte el estado."
                            : response.StatusCode == HttpStatusCode.BadRequest ? "Revise la placa y la confirmacion del encargado."
                            : "No se pudo completar la operacion. Consulte el estado antes de reintentar.";
                        throw new IOException(message);
                    }
                    return response.StatusCode == HttpStatusCode.NoContent ? new JObject() : JObject.Parse(await response.Content.ReadAsStringAsync());
                }
            }
        }
    }
}