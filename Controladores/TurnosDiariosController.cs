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
    public class TurnosDiariosController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static TurnosDiariosController()
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

        public List<TurnosDiarios> consultaByFecha(string fecha)
        {
            try
            {
                var response = _httpClient
                    .GetAsync($"{_apiBaseUrl}/api/TurnosDiarios/ByFecha?fecha={fecha}")
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<List<TurnosDiarios>>(json);

                return apiResponse ?? new List<TurnosDiarios>();
            }
            catch (Exception)
            {
                return new List<TurnosDiarios>();
            }
        }

        public int RegistrarTurnoDiario(TurnosDiarios turno)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(turno);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/TurnosDiarios/RegistrarTurnoDiario", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<TurnosDiarios>(json);

                return apiResponse?.Id ?? 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public int ActualizarTurnoDiario(TurnosDiarios turno)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(turno);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PutAsync($"{_apiBaseUrl}/api/TurnosDiarios/ActualizarTurnoDiario/{turno.Id}", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                // Tu endpoint devuelve NoContent (204), así que no hay body
                return turno.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
