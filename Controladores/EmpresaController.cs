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
using System.Threading;
using System.Threading.Tasks;

namespace CarWash.Controladores
{
    public sealed class EmpresaController
    {
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }) { Timeout = TimeSpan.FromSeconds(30) };
        private readonly string baseUrl = (ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "").TrimEnd('/');
        public static EmpresaDTO? Actual { get; private set; }

        public async Task<EmpresaDTO?> ConsultarAsync(CancellationToken cancellationToken)
        {
            try { return Actual = await SendAsync(HttpMethod.Get, null, cancellationToken); }
            catch { Actual = null; throw; }
        }

        public async Task<EmpresaDTO> GuardarAsync(EmpresaDTO data, CancellationToken cancellationToken)
        {
            if (!clsConnection.EsAdministrador) throw new UnauthorizedAccessException("Solo administradores pueden modificar la empresa.");
            return Actual = await SendAsync(HttpMethod.Put, data, cancellationToken) ?? throw new IOException("Respuesta de empresa no valida.");
        }

        private async Task<EmpresaDTO?> SendAsync(HttpMethod method, EmpresaDTO? data, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(method, baseUrl + "/api/Empresa"))
            {
                clsConnection.AddSessionCredential(request);
                if (data != null) request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                using (var response = await Client.SendAsync(request, cancellationToken))
                {
                    if (response.StatusCode == HttpStatusCode.NoContent && method == HttpMethod.Get) return null;
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            clsConnection.SetApiSession(null, baseUrl, false);
                            throw new UnauthorizedAccessException("La sesion vencio. Inicie sesion nuevamente.");
                        }
                        throw new IOException(response.StatusCode == HttpStatusCode.Conflict
                            ? "La empresa fue modificada. Recargue antes de guardar."
                            : response.StatusCode == HttpStatusCode.Forbidden ? "Solo administradores pueden modificar la empresa."
                            : response.StatusCode == HttpStatusCode.BadRequest ? "Revise los campos obligatorios, NIT y logo (PNG/JPEG hasta 1 MB)."
                            : "Datos de empresa no disponibles. Intente recargar.");
                    }
                    var body = await response.Content.ReadAsStringAsync();
                    if (body.Length > 2_097_152) throw new IOException("Respuesta de empresa demasiado grande.");
                    var result = JObject.Parse(body).ToObject<EmpresaDTO>();
                    if (result == null || result.Revision < 1 || string.IsNullOrWhiteSpace(result.NombreComercial)) throw new IOException("Respuesta de empresa no valida.");
                    return result;
                }
            }
        }
    }
}