using System;
using System.Collections.Generic;

namespace CarWash.DTOs
{
    public sealed class ServicioComprobanteDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
    }

    public sealed class ImpresionComprobanteDTO
    {
        public Guid SolicitudId { get; set; }
        public int IdTurno { get; set; }
        public int AnchoMm { get; set; }
        public string Impresora { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTimeOffset CreadoEn { get; set; }
        public DateTimeOffset ActualizadoEn { get; set; }
        public string? ErrorCodigo { get; set; }
    }

    public sealed class ComprobanteRetiroDTO
    {
        public int IdTurno { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Cliente { get; set; }
        public DateTimeOffset FechaIngreso { get; set; }
        public decimal Total { get; set; }
        public List<ServicioComprobanteDTO> Servicios { get; set; } = new List<ServicioComprobanteDTO>();
        public DateTimeOffset? EntregadoEn { get; set; }
        public int? EntregadoPor { get; set; }
    }
}