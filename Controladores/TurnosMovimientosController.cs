using CarWash.Entidades;
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
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
