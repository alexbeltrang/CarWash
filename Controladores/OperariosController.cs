using CarWash.DTOs;
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
    public class OperariosController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static OperariosController()
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



        public List<OperariosDTO> obtenerOperariosDisponibles()
        {
            try
            {
                var response = _httpClient
                    .GetAsync($"{_apiBaseUrl}/api/Operarios/OperadoresDisponibles")
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var apiResponse = JsonConvert.DeserializeObject<List<OperariosDTO>>(json);

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new List<OperariosDTO>
                {

                };
            }
        }


        public List<Operarios> GetOperariosActivos()
        {
            try
            {
                var response = _httpClient
                    .GetAsync($"{_apiBaseUrl}/api/Operarios/getOperariosActivos")
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var apiResponse = JsonConvert.DeserializeObject<List<Operarios>>(json);

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new List<Operarios>
                {

                };
            }
        }


    }
}
