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
    [Table("ValesOperarios")]
    public class ValesOperarios
    {
        [PrimaryKey, AutoIncrement, Column("IdValeOperario")]
        public int IdValeOperario { get; set; }
        [ForeignKey(typeof(Operarios))]
        public int idOperario { get; set; }
        [ForeignKey(typeof(CajaDiaria))]
        public int idCaja { get; set; }
        public DateTime FechaRegsitro { get; set; }
        public decimal Valor { get; set; }
        public string Motivo { get; set; }
        public bool Descontado { get; set; }
        public DateTime FechaDescuento { get; set; }
    }
}

