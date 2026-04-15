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
    public class TurnosServiciosController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static TurnosServiciosController()
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

        public long RegistrarTurnoServicio(TurnoServicios turno)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(turno);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/TurnoServicios/RegistrarTurnoServicio", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<TurnoServicios>(json);

                return apiResponse?.idTunoServicios ?? 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
