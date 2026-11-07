using System;

namespace CarWash.DTOs
{
    public sealed class ConfiguracionFacturacionDTO
    {
        public int Revision { get; set; }
        public bool Habilitada { get; set; }
        public string Prefijo { get; set; } = "";
        public string NumeroResolucion { get; set; } = "";
        public string FechaResolucion { get; set; } = "";
        public string VigenteDesde { get; set; } = "";
        public string VigenteHasta { get; set; } = "";
        public long DesdeNumero { get; set; }
        public long HastaNumero { get; set; }
        public long SiguienteNumero { get; set; }
        public decimal? TasaIva { get; set; }
    }
    public sealed class CompradorFacturaDTO
    {
        public string Nombre { get; set; } = "";
        public string TipoDocumento { get; set; } = "";
        public string NumeroDocumento { get; set; } = "";
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
    public sealed class PreparacionFacturaDTO
    {
        public int IdTurno { get; set; }
        public string Placa { get; set; } = "";
        public string Concepto { get; set; } = "";
        public decimal Total { get; set; }
        public EmpresaDTO Empresa { get; set; } = new EmpresaDTO();
        public ConfiguracionFacturacionDTO Configuracion { get; set; } = new ConfiguracionFacturacionDTO();
        public CompradorFacturaDTO Comprador { get; set; } = new CompradorFacturaDTO();
    }
    public sealed class FacturaDTO
    {
        public Guid Id { get; set; }
        public int IdTurno { get; set; }
        public string Prefijo { get; set; } = "";
        public long Numero { get; set; }
        public string Fecha { get; set; } = "";
        public DateTimeOffset EmitidaEn { get; set; }
        public string Placa { get; set; } = "";
        public string Concepto { get; set; } = "";
        public int Cantidad { get; set; }
        public EmpresaDTO Empresa { get; set; } = new EmpresaDTO();
        public ConfiguracionFacturacionDTO Configuracion { get; set; } = new ConfiguracionFacturacionDTO();
        public CompradorFacturaDTO Comprador { get; set; } = new CompradorFacturaDTO();
        public decimal Base { get; set; }
        public decimal TasaIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string Texto { get; set; } = "";
    }
}