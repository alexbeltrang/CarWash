using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CarWash.Entidades
{
    [Table("Turnos")]
    public class Turnos
    {
        [PrimaryKey, AutoIncrement, Column("IdTurno")]
        public int IdTurno { get; set; }
        public string NumeroTurno { get; set; }
        public string NombreCliente { get; set; }
        public string NumeroCelular { get; set; }
        public string Placa { get; set; }
        public DateTime FechaHoraIngreso { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string Marca { get; set; }
        public string NumeroOrden { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorBaseComision { get; set; }
        public decimal PorcentajeComision { get; set; }
        public decimal ValorComision { get; set; }
        public bool Pagado { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaHoraAsignacionOperario { get; set; }
        public bool Estado { get; set; }
        [ForeignKey(typeof(TipoVehiculo))]
        public int IdTipoVehiculo { get; set; }
        [ForeignKey(typeof(Servicios))]
        public int idOperario { get; set; }
        public bool OperadorOcupado { get; set; }
        [ForeignKey(typeof(CajaDiaria))]
        public int idCajaDiaria { get; set; }
        [ForeignKey(typeof(ClienteCredito))]
        public int idClienteCredito { get; set; }
    }
}
