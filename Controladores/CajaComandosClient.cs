using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Utilidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarWash.Controladores
{
    public sealed class CajaHttpException : IOException
    {
        public int Status { get; }
        public string Code { get; }
        public bool EscrituraRechazada { get; internal set; }
        public CajaHttpException(int status, string code) : base(MessageFor(code)) { Status = status; Code = code; }
        private static string MessageFor(string code)
        {
            switch (code)
            {
                case "STALE_SUMMARY": return "La jornada cambio. Actualice el resumen y confirme nuevamente.";
                case "STALE_IMPACT": return "El impacto cambio. Consulte otra vez y confirme nuevamente.";
                case "FACTURA_EMITIDA": return "El turno tiene factura emitida. No se permite corregir o borrar.";
                case "DEPENDENCY_LOCKED": return "Hay dependencias activas, inciertas o de nomina. No se permite continuar.";
                case "CAJA_CERRADA": return "La caja esta cerrada.";
                case "REVISION_SOLO_CORRECCION": return "La jornada historica solo admite correcciones de operaciones existentes.";
                case "CAJA_YA_ABIERTA": return "Ya existe una caja abierta.";
                case "INVALID_COMMAND": return "Revise el importe, el medio y los campos obligatorios.";
                case "INVALID_SOURCE_DATA": return "Hay fuentes no verificables. No se puede confirmar.";
                case "MULTIPLE_CAJAS_ACTIVAS": return "Existen varias cajas abiertas. Requiere revision administrativa.";
                case "SESSION_REQUIRED": return "La sesion vencio. Inicie sesion nuevamente.";
                case "ADMIN_REQUIRED": return "No tiene permisos para esta consulta.";
                case "CLIENT_UPDATE_REQUIRED": return "Esta operacion requiere el cliente financiero actualizado.";
                case "IDEMPOTENCY_CONFLICT": return "El identificador corresponde a otra solicitud. Consulte su resultado.";
                default: return "Resultado no confirmado. Consulte el estado antes de reintentar.";
            }
        }
    }

    public sealed class CajaComandosClient
    {
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }) { Timeout = TimeSpan.FromSeconds(30) };
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings { FloatParseHandling = FloatParseHandling.Decimal, DateParseHandling = DateParseHandling.DateTimeOffset };
        private readonly string baseUrl = (ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "").TrimEnd('/');
        private readonly Dictionary<Guid, string> attempted = new Dictionary<Guid, string>();

        public static string Dinero(decimal amount) => amount.ToString("0.############################", CultureInfo.InvariantCulture);
        public Task<CajaDiaria?> ActivaAsync(CancellationToken token) => SendAsync<CajaDiaria>(HttpMethod.Get, "api/CajaDiaria/getCajaActiva", null, token, true);
        public async Task<CajaResumenDTO> ResumenAsync(int id, CancellationToken token)
        {
            var result = await SendAsync<CajaResumenDTO>(HttpMethod.Get, "api/CajaDiaria/" + id + "/resumen", null, token);
            if (result == null || result.IdCaja != id || result.Revision < 0 || result.ResumenToken.Length != 64
                || (result.Estado != "ABIERTA" && result.Estado != "CERRADA" && result.Estado != "EN_REVISION")) throw new IOException("Resumen no verificable.");
            return result;
        }
        public async Task<List<CajaMedioDTO>> MediosAsync(CancellationToken token) =>
            await SendAsync<List<CajaMedioDTO>>(HttpMethod.Get, "api/FormaPago/GetAll", null, token) ?? throw new IOException("Catalogo no disponible.");
        public async Task<CajaResultadoDTO?> OperacionAsync(Guid id, CancellationToken token)
        {
            var result = await SendAsync<CajaResultadoDTO>(HttpMethod.Get, "api/CajaDiaria/operaciones/" + id, null, token, true);
            if (result != null) Validate(result, id);
            return result;
        }
        public Task<CajaResultadoDTO> AbrirAsync(CajaAperturaDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/apertura", command, token);
        public Task<CajaResultadoDTO> CerrarAsync(int id, CajaCierreDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/" + id + "/cierre", command, token);
        public Task<CajaResultadoDTO> RecerrarAsync(int id, CajaCierreDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/" + id + "/recierre", command, token);
        public Task<CajaResultadoDTO> ReabrirAsync(int id, CajaReaperturaDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/" + id + "/reapertura", command, token);
        public async Task<CajaPaginaDTO<CajaCierreVersionDTO>> CierresAsync(DateTime desde, DateTime hasta, int pagina, CancellationToken token) =>
            await SendAsync<CajaPaginaDTO<CajaCierreVersionDTO>>(HttpMethod.Get, "api/CajaDiaria/cierres?desde=" + desde.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                + "&hasta=" + hasta.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "&pagina=" + pagina + "&tamanoPagina=50", null, token) ?? throw new IOException("Historico no disponible.");
        public async Task<CajaCierreVersionDTO> CierreAsync(int id, int version, CancellationToken token) =>
            await SendAsync<CajaCierreVersionDTO>(HttpMethod.Get, "api/CajaDiaria/" + id + "/cierres/" + version, null, token) ?? throw new IOException("Version no disponible.");
        public Task<CajaResultadoDTO> CobrarAsync(int id, CajaCobroDTO command, CancellationToken token) => CommandAsync("api/Turnos/" + id + "/cobros", command, token);
        public Task<CajaResultadoDTO> GastoAsync(int id, CajaGastoDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/" + id + "/gastos", command, token);
        public Task<CajaResultadoDTO> ValeAsync(int id, CajaValeDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/" + id + "/vales", command, token);
        public Task<CajaResultadoDTO> OperarioAsync(int id, CajaOperarioDTO command, CancellationToken token) => CommandAsync("api/Turnos/" + id + "/operario", command, token);
        public async Task<CajaPaginaDTO<CajaMovimientoDTO>> MovimientosAsync(int id, int pagina, CancellationToken token) =>
            await SendAsync<CajaPaginaDTO<CajaMovimientoDTO>>(HttpMethod.Get, "api/CajaDiaria/" + id + "/movimientos?pagina=" + pagina + "&tamanoPagina=50", null, token) ?? throw new IOException("Movimientos no disponibles.");
        public async Task<CajaImpactoDTO> ImpactoAsync(int id, bool turno, CancellationToken token)
        {
            var result = await SendAsync<CajaImpactoDTO>(HttpMethod.Get, "api/" + (turno ? "Turnos/" + id + "/impacto-borrado" : "TurnosMovimientos/" + id + "/impacto"), null, token);
            if (result == null || result.ImpactoToken.Length != 64 || result.Cajas.Count == 0) throw new IOException("Impacto no verificable.");
            return result;
        }
        public Task<CajaResultadoDTO> CorregirCobroAsync(int id, CajaCorreccionCobroDTO command, CancellationToken token) => CommandAsync("api/TurnosMovimientos/" + id + "/correccion", command, token);
        public Task<CajaResultadoDTO> BorrarAsync(int id, bool turno, CajaBorradoDTO command, CancellationToken token) => CommandAsync("api/" + (turno ? "Turnos/" : "TurnosMovimientos/") + id + "/borrado", command, token);
        public Task<CajaResultadoDTO> CorregirSalidaAsync(int idCaja, int id, bool vale, CajaCorreccionSalidaDTO command, CancellationToken token) => CommandAsync("api/CajaDiaria/" + idCaja + (vale ? "/vales/" : "/gastos/") + id + "/correccion", command, token);
        public async Task<CajaSaldoDTO> SaldoAsync(int id, CancellationToken token)
        {
            var result = await SendAsync<CajaSaldoDTO>(HttpMethod.Get, "api/Turnos/" + id + "/saldo", null, token);
            if (result == null || result.IdTurno != id || result.IdCajaOrigen <= 0 || result.SaldoPendiente < 0) throw new IOException("Saldo no verificable.");
            return result;
        }

        public static string Categoria(string name)
        {
            var normalized = new StringBuilder();
            foreach (var character in (name ?? "").Normalize(NormalizationForm.FormD))
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark) normalized.Append(character);
            var value = normalized.ToString().Trim().ToUpperInvariant();
            return value == "TRANSFERENCIAS" ? "TRANSFERENCIA" : value;
        }

        private async Task<CajaResultadoDTO> CommandAsync(string path, CajaComandoDTO command, CancellationToken token)
        {
            if (command.IdOperacion == Guid.Empty || !command.Confirmar) throw new ArgumentException("Comando no confirmado.");
            var payload = path + "\n" + JsonConvert.SerializeObject(command);
            if (attempted.TryGetValue(command.IdOperacion, out var original))
            {
                if (original != payload) throw new CajaHttpException(409, "IDEMPOTENCY_CONFLICT");
                var previous = await OperacionAsync(command.IdOperacion, token);
                if (previous != null) return previous;
            }
            attempted[command.IdOperacion] = payload;
            CajaResultadoDTO result;
            try { result = await SendAsync<CajaResultadoDTO>(HttpMethod.Post, path, command, token) ?? throw new IOException("Resultado no confirmado."); }
            catch (CajaHttpException error)
            {
                error.EscrituraRechazada = (error.Status == 400 || error.Status == 409) && error.Code != "IDEMPOTENCY_CONFLICT";
                throw;
            }
            Validate(result, command.IdOperacion);
            return result;
        }
        private static void Validate(CajaResultadoDTO result, Guid id)
        {
            if (result.IdOperacion != id || result.HttpStatus < 200 || result.HttpStatus >= 300 || result.ConfirmadaEnUtc == default(DateTimeOffset)
                || result.Resultado == null || result.Revisiones == null || result.Revisiones.Count == 0)
                throw new IOException("Resultado no confirmado. Conserve el identificador original.");
        }
        private async Task<T?> SendAsync<T>(HttpMethod method, string path, object? data, CancellationToken token, bool missingAllowed = false) where T : class
        {
            using (var request = new HttpRequestMessage(method, baseUrl + "/" + path))
            {
                clsConnection.AddSessionCredential(request);
                if (data != null) request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                using (var response = await Client.SendAsync(request, token))
                {
                    var text = await response.Content.ReadAsStringAsync();
                    if (text.Length > 16_000_000) throw new IOException("Respuesta de caja no valida.");
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    { clsConnection.SetApiSession(null, baseUrl, false); throw new CajaHttpException(401, "SESSION_REQUIRED"); }
                    if (missingAllowed && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        try
                        {
                            var code = JObject.Parse(text).Value<string>("code");
                            if (code == (path.Contains("/operaciones/") ? "OPERATION_UNKNOWN" : "NOT_FOUND")) return null;
                        }
                        catch (JsonException) { }
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        string code = response.StatusCode == HttpStatusCode.Forbidden ? "ADMIN_REQUIRED" : "CAJA_UNAVAILABLE";
                        try { code = JObject.Parse(text).Value<string>("code") ?? code; } catch (JsonException) { }
                        throw new CajaHttpException((int)response.StatusCode, code);
                    }
                    try { return JsonConvert.DeserializeObject<T>(text, Settings) ?? throw new IOException("Respuesta de caja no valida."); }
                    catch (JsonException) { throw new IOException("Respuesta de caja no valida."); }
                }
            }
        }
    }
}