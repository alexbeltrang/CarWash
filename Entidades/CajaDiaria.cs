using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("CajaDiaria")]
    public class CajaDiaria
    {
        [PrimaryKey, AutoIncrement, Column("idCaja")]
        public int idCaja { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
        public decimal MontoInicial { get; set; }
        public decimal TotalIngresosEfectivo { get; set; }
        public decimal TotalIngresosTransferencias { get; set; }
        public decimal TotalIngresosDatafono { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal TotalFinal { get; set; }
        public bool Estado { get; set; }
    }
}

