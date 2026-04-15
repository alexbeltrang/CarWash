using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("LiquidacionOperario")]
    public class LiquidacionOperario
    {
        [PrimaryKey, AutoIncrement, Column("idLiquidacionOperario")]
        public int idLiquidacionOperario { get; set; }
        [ForeignKey(typeof(Operarios))]
        public int idOperario { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int TotalServicios { get; set; }
        public decimal TotalFacturado { get; set; }
        public decimal Comision { get; set; }
        public decimal Vales { get; set; }
        public decimal TotalPagado { get; set; }
        public DateTime FechaLiquidacion { get; set; }
    }
}
