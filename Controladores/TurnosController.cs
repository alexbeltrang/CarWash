using CarWash.DTOs;
using CarWash.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Controladores
{
    public class TurnosController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static TurnosController()
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

        public Turnos consultaTurnoId(int IdTurno)
        {
            try
            {

                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/GetById?id={IdTurno}")
                   .GetAwaiter().GetResult();


                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<Turnos>(json);

                return apiResponse ?? new Turnos();
            }
            catch (Exception ex)
            {
                return new Turnos();
            }
        }


        public int ActualizarTurno(Turnos turno)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(turno);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PutAsync($"{_apiBaseUrl}/api/Turnos/ActualizarTurno/{turno.IdTurno}", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                // Tu endpoint devuelve NoContent (204), así que no hay body
                return turno.IdTurno;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public int RegistrarTurno(Turnos turno)
        {
            try
            {
                var requestBody = JsonConvert.SerializeObject(turno);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/Turnos/RegistrarTurno", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<Turnos>(json);

                return apiResponse?.IdTurno ?? 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Turnos BuscarByPlaca(string placa)
        {
            try
            {
                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/BuscarPorPlaca?placa={placa}")
                   .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<Turnos>(json);

                return apiResponse;
            }
            catch (Exception)
            {
                return new Turnos();
            }
        }
        public List<ServicioNominaDTO> PendientesNomina(
            int idOperario,
            DateTime fechaInicial,
            DateTime fechaFinal)
        {
            try
            {
                string url = $"{_apiBaseUrl}/api/Turnos/PendientesNomina" +
                             $"?idOperario={idOperario}" +
                             $"&fechaInicial={fechaInicial:yyyy-MM-dd}" +
                             $"&fechaFinal={fechaFinal:yyyy-MM-dd}";

                var response = _httpClient
                    .GetAsync(url)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var apiResponse = JsonConvert.DeserializeObject<List<ServicioNominaDTO>>(json);

                return apiResponse ?? new List<ServicioNominaDTO>();
            }
            catch (Exception)
            {
                return new List<ServicioNominaDTO>();
            }
        }
        public List<IngresoVehiculoDTO> HistoricoByPlaca(string placa)
        {
            try
            {
                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/HistorialPorPlaca?placa={placa}")
                   .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<List<IngresoVehiculoDTO>>(json);

                return apiResponse;
            }
            catch (Exception)
            {
                return new List<IngresoVehiculoDTO>();
            }
        }
        public List<GestionVehiculosDTO> VehiculoEnProceso(int idTurno)
        {
            try
            {
                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/VehiculoEnProceso?idTurno={idTurno}")
                   .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<GestionVehiculosDTO>(json);

                return apiResponse != null
                    ? new List<GestionVehiculosDTO> { apiResponse }
                    : new List<GestionVehiculosDTO>();
            }
            catch (Exception)
            {
                return new List<GestionVehiculosDTO>();
            }
        }
        public List<GestionVehiculosDTO> VehiculosEnProceso()
        {
            try
            {
                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/VehiculosEnProceso")
                   .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<List<GestionVehiculosDTO>>(json);

                return apiResponse ?? new List<GestionVehiculosDTO>();
            }
            catch (Exception)
            {
                return new List<GestionVehiculosDTO>();
            }
        }

        public List<DashboardOperadoresDTO> DashboardOperadores()
        {
            try
            {
                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/DashboardOperadores")
                   .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<List<DashboardOperadoresDTO>>(json);

                return apiResponse ?? new List<DashboardOperadoresDTO>();
            }
            catch (Exception)
            {
                return new List<DashboardOperadoresDTO>();
            }
        }


        public List<Turnos> GetallActivos()
        {
            try
            {
                var response = _httpClient
                   .GetAsync($"{_apiBaseUrl}/api/Turnos/GetallActivos")
                   .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiResponse = JsonConvert.DeserializeObject<List<Turnos>>(json);

                return apiResponse ?? new List<Turnos>();
            }
            catch (Exception)
            {
                return new List<Turnos>();
            }
        }
    }
}
