using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("TurnosMovimientos")]
    public class TurnosMovimientos
    {
        [PrimaryKey, AutoIncrement, Column("IdTurnoMovimientos")]
        public int IdTurnoMovimientos { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaMovimiento { get; set; }
        [ForeignKey(typeof(Turnos))]
        public int IdTurno { get; set; }
        [ForeignKey(typeof(FormaPago))]
        public int IdFormaPago { get; set; }

    }
}
