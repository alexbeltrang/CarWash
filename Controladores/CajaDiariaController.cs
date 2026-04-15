using CarWash.Entidades;
using CarWash.ModelosRespuestas;
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
    public class CajaDiariaController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static CajaDiariaController()
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


        public CajaDiaria GetCajaActiva()
        {
            try
            {
                var response = _httpClient
                    .GetAsync($"{_apiBaseUrl}/api/CajaDiaria/getCajaActiva")
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var apiResponse = JsonConvert.DeserializeObject<CajaDiaria>(json);

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new CajaDiaria
                {

                };
            }
        }


        public int ActualizarCajaDiaria(CajaDiaria cajaDiaria)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(cajaDiaria);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PutAsync($"{_apiBaseUrl}/api/CajaDiaria/ActualizarCajaDiaria/{cajaDiaria.idCaja}", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                // Tu endpoint devuelve NoContent (204), así que no hay body
                return cajaDiaria.idCaja;
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }
}
