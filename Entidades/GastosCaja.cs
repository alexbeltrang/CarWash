using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("GastosCaja")]
    public class GastosCaja
    {
        [PrimaryKey, AutoIncrement, Column("IdGasto")]
        public int IdGasto { get; set; }
        [ForeignKey(typeof(CajaDiaria))]
        public int idCaja { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Concepto { get; set; }
        public decimal Valor { get; set; }
        public string Observaciones { get; set; }
    }
}
