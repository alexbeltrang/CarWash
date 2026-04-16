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
    public class AsistenciaOperarioController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static AsistenciaOperarioController()
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

        public AsistenciaOperario consultaAsistenciaOperarioId(int id)
        {
            try
            {

                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/AsistenciaOperario/GetById?id={id}")
                   .GetAwaiter().GetResult();


                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<AsistenciaOperario>(json);

                return apiResponse ?? new AsistenciaOperario();
            }
            catch (Exception ex)
            {
                return new AsistenciaOperario();
            }
        }


        public int ActualizarAsistenciaOperario(AsistenciaOperario asistencia)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(asistencia);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PutAsync($"{_apiBaseUrl}/api/AsistenciaOperario/ActualizarAsistenciaOperario/{asistencia.idAsistenciaOperario}", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                // Tu endpoint devuelve NoContent (204), así que no hay body
                return asistencia.idAsistenciaOperario;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public int RegistrarAsistenciaOperario(AsistenciaOperario asistencia)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(asistencia);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/AsistenciaOperario/RegistrarAsistenciaOperario", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<AsistenciaOperario>(json);

                return apiResponse?.idAsistenciaOperario ?? 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}