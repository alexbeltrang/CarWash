using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CarWash.DTOs
{
    public class CajaComandoDTO
    {
        public Guid IdOperacion { get; set; } = Guid.NewGuid();
        public bool Confirmar { get; set; } = true;
    }
    public sealed class CajaAperturaDTO : CajaComandoDTO
    {
        public string MontoInicial { get; set; } = "0";
    }
    public sealed class CajaCierreDTO : CajaComandoDTO
    {
        public long RevisionEsperada { get; set; }
        public string ResumenToken { get; set; } = "";
        public string EfectivoContado { get; set; } = "0";
        public string? JustificacionDiferencia { get; set; }
    }
    public sealed class CajaReaperturaDTO : CajaComandoDTO
    {
        public long RevisionEsperada { get; set; }
        public int VersionEsperada { get; set; }
        public string Motivo { get; set; } = "";
    }
    public sealed class CajaCierreVersionDTO
    {
        [System.ComponentModel.Browsable(false)]
        public Guid Id { get; set; }
        public int IdCaja { get; set; }
        public int Version { get; set; }
        public string Origen { get; set; } = "";
        public bool Vigente { get; set; }
        public bool EnRevision { get; set; }
        public long Revision { get; set; }
        public string? Responsable { get; set; }
        public DateTimeOffset? ConfirmadoEnUtc { get; set; }
        public string? EfectivoContado { get; set; }
        public string? Diferencia { get; set; }
        public string? JustificacionDiferencia { get; set; }
        public string? MotivoRevision { get; set; }
        [System.ComponentModel.Browsable(false)]
        public JObject Snapshot { get; set; } = new JObject();
    }
    public sealed class CajaCobroDTO : CajaComandoDTO
    {
        public int IdCaja { get; set; }
        public long RevisionEsperada { get; set; }
        public string Importe { get; set; } = "0";
        public int IdFormaPago { get; set; }
        public bool Finalizar { get; set; }
        public int? IdClienteCredito { get; set; }
        public string? Observaciones { get; set; }
    }
    public sealed class CajaGastoDTO : CajaComandoDTO
    {
        public long RevisionEsperada { get; set; }
        public string Importe { get; set; } = "0";
        public int IdFormaPagoSalida { get; set; }
        public string Concepto { get; set; } = "";
        public string? Observaciones { get; set; }
    }
    public sealed class CajaValeDTO : CajaComandoDTO
    {
        public long RevisionEsperada { get; set; }
        public string Importe { get; set; } = "0";
        public int IdFormaPagoSalida { get; set; }
        public int IdOperario { get; set; }
        public string Motivo { get; set; } = "";
    }
    public sealed class CajaOperarioDTO : CajaComandoDTO
    {
        public int IdCaja { get; set; }
        public long RevisionEsperada { get; set; }
        public int? IdOperario { get; set; }
        public bool Liberar { get; set; }
    }
    public sealed class CajaSaldoDTO
    {
        public int IdTurno { get; set; }
        public int IdCajaOrigen { get; set; }
        public decimal SaldoPendiente { get; set; }
        public bool Finalizado { get; set; }
        public bool Pagado { get; set; }
    }
    public sealed class CajaResultadoDTO
    {
        public Guid IdOperacion { get; set; }
        public int HttpStatus { get; set; }
        public JObject Resultado { get; set; } = new JObject();
        public Dictionary<int, long> Revisiones { get; set; } = new Dictionary<int, long>();
        public DateTimeOffset ConfirmadaEnUtc { get; set; }
    }
    public sealed class CajaRevisionDTO
    {
        public int IdCaja { get; set; }
        public long Revision { get; set; }
        public int VersionVigente { get; set; }
    }
    public sealed class CajaMovimientoDTO
    {
        public int Id { get; set; }
        public int IdTurno { get; set; }
        public int? IdCaja { get; set; }
        public DateTimeOffset? Fecha { get; set; }
        public decimal? Importe { get; set; }
        public int IdFormaPago { get; set; }
        public string Categoria { get; set; } = "";
        public string Naturaleza { get; set; } = "";
    }
    public sealed class CajaPaginaDTO<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Pagina { get; set; }
        public int TamanoPagina { get; set; }
        public long TotalCount { get; set; }
        public List<CajaFuenteDTO> FuentesInvalidas { get; set; } = new List<CajaFuenteDTO>();
    }
    public sealed class CajaImpactoDTO
    {
        public string ImpactoToken { get; set; } = "";
        public List<CajaRevisionDTO> Cajas { get; set; } = new List<CajaRevisionDTO>();
        public Dictionary<string, int[]> Identificadores { get; set; } = new Dictionary<string, int[]>();
        public List<CajaFuenteDTO> Bloqueos { get; set; } = new List<CajaFuenteDTO>();
        public List<CajaMovimientoDTO> Movimientos { get; set; } = new List<CajaMovimientoDTO>();
        public List<string> Trabajos { get; set; } = new List<string>();
    }
    public sealed class CajaCorreccionCobroDTO : CajaComandoDTO
    {
        public long RevisionEsperada { get; set; }
        public Dictionary<int, long> RevisionesEsperadas { get; set; } = new Dictionary<int, long>();
        public string ImpactoToken { get; set; } = "";
        public string Importe { get; set; } = "0";
        public int IdFormaPago { get; set; }
        public string Motivo { get; set; } = "";
    }
    public sealed class CajaBorradoDTO : CajaComandoDTO
    {
        public Dictionary<int, long> RevisionesEsperadas { get; set; } = new Dictionary<int, long>();
        public string ImpactoToken { get; set; } = "";
        public string Motivo { get; set; } = "";
    }
    public sealed class CajaCorreccionSalidaDTO : CajaComandoDTO
    {
        public long RevisionEsperada { get; set; }
        public string Importe { get; set; } = "0";
        public int IdFormaPagoSalida { get; set; }
        public string Motivo { get; set; } = "";
    }
    public sealed class CajaImporteDTO
    {
        public int IdFormaPago { get; set; }
        public string Nombre { get; set; } = "";
        public string Categoria { get; set; } = "";
        public decimal Importe { get; set; }
    }
    public sealed class CajaMedioDTO
    {
        public int IdFormaPago { get; set; }
        public string Nombre { get; set; } = "";
    }
    public sealed class CajaFuenteDTO
    {
        public string Fuente { get; set; } = "";
        public int? Id { get; set; }
        public string Codigo { get; set; } = "";
    }
    public sealed class CajaResumenDTO
    {
        public int IdCaja { get; set; }
        public string Estado { get; set; } = "";
        public long Revision { get; set; }
        public string ResumenToken { get; set; } = "";
        public decimal Apertura { get; set; }
        public List<CajaImporteDTO> CobrosPorMedio { get; set; } = new List<CajaImporteDTO>();
        public decimal CreditoPendiente { get; set; }
        public List<CajaImporteDTO> GastosPorMedio { get; set; } = new List<CajaImporteDTO>();
        public List<CajaImporteDTO> ValesPorMedio { get; set; } = new List<CajaImporteDTO>();
        public decimal EfectivoEsperado { get; set; }
        public List<string> DiscrepanciasAcumulados { get; set; } = new List<string>();
        public List<CajaFuenteDTO> FuentesInvalidas { get; set; } = new List<CajaFuenteDTO>();
        public bool PuedeCerrar { get; set; }
    }
}