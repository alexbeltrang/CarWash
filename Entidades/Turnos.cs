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
        public decimal Valor { get; set; }
        public bool Pagado { get; set; }
        public string Observaciones { get; set; }
        public bool Estado { get; set; }
        [ForeignKey(typeof(TipoVehiculo))]
        public int IdTipoVehiculo { get; set; }
        [ForeignKey(typeof(Servicios))]
        public int idServicio { get; set; }
        [ForeignKey(typeof(Operarios))]
        public int idOperario { get; set; }
    }
}
