using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Utilidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Controladores
{
    public class TurnosMovimientosController
    {
        private static readonly HttpClient _httpClient;
        private static readonly HttpClient _readClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static TurnosMovimientosController()
        {
            // Habilitar TLS 1.2 y TLS 1.3 requeridos para HTTPS en .NET Framework
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | (SecurityProtocolType)12288;

            // Ignorar validación del certificado SSL en entornos de desarrollo
            bool ignorarSSL;
            if (bool.TryParse(ConfigurationManager.AppSettings["ApiIgnorarSSL"], out ignorarSSL) && ignorarSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    (sender, cert, chain, errors) => true;
            }

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public int RegistrarTurnoMovimiento(TurnosMovimientos turnoMovimiento)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(turnoMovimiento);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/TurnosMovimientos/RegistrarTurnoMovimiento", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<TurnosMovimientos>(json);

                return apiResponse?.IdTurnoMovimientos ?? 0;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al registrar el turno de movimiento: {ex.Message}");
                return 0;
            }
        }


        public List<ConsultaMovimientosDTO> GetHistoricoMovimientos(
            DateTime fechaInicial,
            DateTime fechaFinal,
            int? idFormaPago,
            int? idOperario)
        {
            try
            {
                if (!clsConnection.EsAdministrador)
                    throw new System.Security.SecurityException("Solo el perfil Administrador puede consultar ventas.");
                string url = $"{_apiBaseUrl}/api/TurnosMovimientos/GetHistoricoMovimientos" +
                             $"?fechaInicial={fechaInicial:yyyy-MM-dd}" +
                             $"&fechaFinal={fechaFinal:yyyy-MM-dd}";

                if (idFormaPago.HasValue)
                    url += $"&idFormaPago={idFormaPago}";

                if (idOperario.HasValue)
                    url += $"&idOperario={idOperario}";

                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    clsConnection.AddSessionCredential(request);
                    using (var response = _readClient.SendAsync(request).GetAwaiter().GetResult())
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            clsConnection.SetApiSession(null, _apiBaseUrl, false);
                            throw new UnauthorizedAccessException("La sesion vencio. Inicie sesion de nuevo para consultar ventas.");
                        }
                        if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            clsConnection.SetApiSession(null, _apiBaseUrl, false);
                            throw new System.Security.SecurityException("Solo el perfil Administrador puede consultar ventas.");
                        }
                        response.EnsureSuccessStatusCode();
                        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return JsonConvert.DeserializeObject<List<ConsultaMovimientosDTO>>(json) ?? new List<ConsultaMovimientosDTO>();
                    }
                }
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (System.Security.SecurityException) { throw; }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo consultar el historico. Intente de nuevo.", ex);
            }
        }


    }
}
