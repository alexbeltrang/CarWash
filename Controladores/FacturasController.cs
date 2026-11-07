using CarWash.DTOs;
using CarWash.Utilidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarWash.Controladores
{
    public sealed class FacturasController
    {
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }) { Timeout = TimeSpan.FromSeconds(30) };
        private readonly string baseUrl = (ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "").TrimEnd('/');
        public Task<ConfiguracionFacturacionDTO?> ConfiguracionAsync(CancellationToken token) => SendAsync<ConfiguracionFacturacionDTO>(HttpMethod.Get, "configuracion", null, token);
        public async Task<ConfiguracionFacturacionDTO> GuardarConfiguracionAsync(ConfiguracionFacturacionDTO data, CancellationToken token)
        {
            if (!clsConnection.EsAdministrador) throw new UnauthorizedAccessException("Solo administradores pueden modificar la facturacion.");
            return await SendAsync<ConfiguracionFacturacionDTO>(HttpMethod.Put, "configuracion", data, token) ?? throw new IOException("Configuracion no valida.");
        }
        public async Task<PreparacionFacturaDTO> PrepararAsync(int? idTurno, string? placa, CancellationToken token) =>
            await SendAsync<PreparacionFacturaDTO>(HttpMethod.Get, "preparacion?" + (idTurno.HasValue ? "idTurno=" + idTurno.Value : "placa=" + Uri.EscapeDataString(placa ?? "")), null, token)
            ?? throw new IOException("Preparacion no valida.");
        public Task<FacturaDTO?> ConsultarAsync(int idTurno, CancellationToken token) => SendAsync<FacturaDTO>(HttpMethod.Get, "turnos/" + idTurno, null, token);
        public async Task<List<FacturaDTO>> HistorialAsync(CancellationToken token, string? placa = null) =>
            await SendAsync<List<FacturaDTO>>(HttpMethod.Get, string.IsNullOrWhiteSpace(placa) ? "" : "?placa=" + Uri.EscapeDataString(placa), null, token) ?? new List<FacturaDTO>();
        public async Task<ImpresionComprobanteDTO?> UltimaImpresionAsync(FacturaDTO invoice, CancellationToken token)
        {
            var job = await SendAsync<ImpresionComprobanteDTO>(HttpMethod.Get, invoice.Id + "/impresiones/ultima", null, token);
            if (job != null && !ValidPrint(job, invoice.IdTurno)) throw new IOException("Estado de impresion no valido. Consulte nuevamente.");
            return job;
        }
        public async Task<FacturaDTO> EmitirAsync(PreparacionFacturaDTO preparation, CompradorFacturaDTO buyer, Guid solicitudId, CancellationToken token)
        {
            var result = await SendAsync<FacturaDTO>(HttpMethod.Post, "turnos/" + preparation.IdTurno, new
            { solicitudId, empresaRevision = preparation.Empresa.Revision, configuracionRevision = preparation.Configuracion.Revision, comprador = buyer }, token);
            if (result == null || result.Id == Guid.Empty || result.IdTurno != preparation.IdTurno || result.Total != preparation.Total)
                throw new IOException("Factura no confirmada. Consulte el turno antes de reintentar.");
            return result;
        }
        public async Task<ImpresionComprobanteDTO> ImprimirAsync(FacturaDTO invoice, Guid solicitudId, CancellationToken token)
        {
            var result = await SendAsync<ImpresionComprobanteDTO>(HttpMethod.Post, invoice.Id + "/impresiones", new { solicitudId, anchoMm = 80 }, token);
            if (result == null || result.SolicitudId != solicitudId || !ValidPrint(result, invoice.IdTurno))
                throw new IOException("Impresion no confirmada. Consulte su estado.");
            return result;
        }
        private static bool ValidPrint(ImpresionComprobanteDTO job, int idTurno) => job.SolicitudId != Guid.Empty && job.IdTurno == idTurno && job.AnchoMm == 80
            && !string.IsNullOrWhiteSpace(job.Impresora) && Array.IndexOf(new[] { "PENDING", "SUBMITTING", "SUBMITTED", "FAILED", "UNKNOWN" }, job.Estado) >= 0;
        private async Task<T?> SendAsync<T>(HttpMethod method, string path, object? data, CancellationToken token) where T : class
        {
            using (var request = new HttpRequestMessage(method, baseUrl + "/api/Facturas" + (path.Length == 0 || path.StartsWith("?", StringComparison.Ordinal) ? "" : "/") + path))
            {
                clsConnection.AddSessionCredential(request);
                if (data != null) request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                using (var response = await Client.SendAsync(request, token))
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    { clsConnection.SetApiSession(null, baseUrl, false); throw new UnauthorizedAccessException("La sesion vencio. Inicie sesion nuevamente."); }
                    var text = await response.Content.ReadAsStringAsync();
                    if (text.Length > 32_000_000) throw new IOException("Respuesta de facturacion demasiado grande.");
                    if (!response.IsSuccessStatusCode)
                    {
                        var message = response.StatusCode == HttpStatusCode.Forbidden ? "Solo administradores pueden modificar la facturacion."
                            : "No se pudo completar la facturacion. Consulte el estado antes de reintentar.";
                        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Conflict)
                        {
                            try { var title = JObject.Parse(text).Value<string>("title"); if (title != null && title.Length <= 300 && !title.Any(char.IsControl)) message = title; }
                            catch (JsonException) { }
                        }
                        throw new IOException(message);
                    }
                    return response.StatusCode == HttpStatusCode.NoContent ? null : JsonConvert.DeserializeObject<T>(text)
                        ?? throw new IOException("Respuesta de facturacion no valida.");
                }
            }
        }
    }
}