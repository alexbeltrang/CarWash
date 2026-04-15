using CarWash.Database;
using CarWash.Entidades;
using CarWash.ModelosRespuestas;
using CarWash.Utilidades;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CarWash.Controladores
{
    public class LoginController
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        static LoginController()
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

        public bool ValidarCampos(string usuario, string password, out string mensaje)
        {
            if (string.IsNullOrEmpty(usuario))
            {
                mensaje = "Digite el Usuario";
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                mensaje = "Digite la Contraseña";
                return false;
            }

            mensaje = string.Empty;
            return true;
        }

        public RespuestaUsuarioLogin Login(string usuario, string password)
        {
            try
            {
                string usrCif = FunctionsEncrip.Cifrado(1, usuario);
                string pwdCif = FunctionsEncrip.Cifrado(1, password);

                var requestBody = JsonConvert.SerializeObject(new { UserName = usrCif, Password = pwdCif });
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync($"{_apiBaseUrl}/api/auth/login", content)
                    .GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var apiResponse = JsonConvert.DeserializeObject<LoginApiResponse>(json);

                if (!apiResponse.EsValido)
                    return new RespuestaUsuarioLogin { esValido = false, respuesta = apiResponse.Respuesta };

                return new RespuestaUsuarioLogin
                {
                    esValido = true,
                    respuesta = apiResponse.Respuesta,
                    Usuario = new Usuario
                    {
                        idUser = apiResponse.Usuario.IdUser,
                        DisplayName = apiResponse.Usuario.DisplayName,
                        Email = apiResponse.Usuario.Email,
                        PerfilId = apiResponse.Usuario.PerfilId
                    }
                };
            }
            catch (Exception ex)
            {
                return new RespuestaUsuarioLogin
                {
                    esValido = false,
                    respuesta = "Error conectando con el servidor: " + ex.Message
                };
            }
        }

        public void InicializarBaseDeDatos()
        {
            DatabaseHelper.CreateOrUpdateTable<Festivos>();
            DatabaseHelper.CreateOrUpdateTable<Modulo>();
            DatabaseHelper.CreateOrUpdateTable<TurnosDiarios>();
            DatabaseHelper.CreateOrUpdateTable<Usuario>();
            DatabaseHelper.CreateOrUpdateTable<OperarioComisiones>();
            DatabaseHelper.CreateOrUpdateTable<CajaDiaria>();
            DatabaseHelper.CreateOrUpdateTable<FormaPago>();
            DatabaseHelper.CreateOrUpdateTable<Operarios>();
            DatabaseHelper.CreateOrUpdateTable<Perfil>();
            DatabaseHelper.CreateOrUpdateTable<PerfilModulo>();
            DatabaseHelper.CreateOrUpdateTable<Servicios>();
            DatabaseHelper.CreateOrUpdateTable<TipoVehiculo>();
            DatabaseHelper.CreateOrUpdateTable<Turnos>();
            DatabaseHelper.CreateOrUpdateTable<TurnosMovimientos>();
            DatabaseHelper.CreateOrUpdateTable<PrecioServicioVehiculo>();
            DatabaseHelper.CreateOrUpdateTable<JornadaOperario>();
            DatabaseHelper.CreateOrUpdateTable<ClienteCredito>();
            DatabaseHelper.CreateOrUpdateTable<TurnoServicios>();
            DatabaseHelper.CreateOrUpdateTable<ValesOperarios>();
            DatabaseHelper.CreateOrUpdateTable<GastosCaja>();
            DatabaseHelper.CreateOrUpdateTable<RegistroPropinasOperarios>();
            DatabaseHelper.CreateOrUpdateTable<AsistenciaOperario>();
            DatabaseHelper.CreateOrUpdateTable<LiquidacionOperario>();
        }

        private class LoginApiResponse
        {
            public bool EsValido { get; set; }
            public string Respuesta { get; set; }
            public UsuarioApiResponse Usuario { get; set; }
        }

        private class UsuarioApiResponse
        {
            public int IdUser { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public int PerfilId { get; set; }
        }
    }
}

