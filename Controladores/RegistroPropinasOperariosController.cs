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
    public class RegistroPropinasOperariosController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static RegistroPropinasOperariosController()
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

        public RegistroPropinasOperarios consultaRegistroPropinasId(int id)
        {
            try
            {

                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/RegistroPropinasOperarios/GetById?id={id}")
                   .GetAwaiter().GetResult();


                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<RegistroPropinasOperarios>(json);

                return apiResponse ?? new RegistroPropinasOperarios();
            }
            catch (Exception ex)
            {
                return new RegistroPropinasOperarios();
            }
        }


        public int ActualizarRegistroPropinas(RegistroPropinasOperarios registro)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(registro);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PutAsync($"{_apiBaseUrl}/api/RegistroPropinasOperarios/ActualizarRegistroPropinas/{registro.idPropina}", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                // Tu endpoint devuelve NoContent (204), así que no hay body
                return registro.idPropina;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public int RegistrarRegistroPropinas(RegistroPropinasOperarios registro)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(registro);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/RegistroPropinasOperarios/RegistrarRegistroPropinasOperarios", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<RegistroPropinasOperarios>(json);

                return apiResponse?.idPropina ?? 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
