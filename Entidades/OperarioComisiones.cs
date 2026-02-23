using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("OperarioComisiones")]
    public class OperarioComisiones
    {
        [PrimaryKey, AutoIncrement, Column("IdOperarioComision")]
        public int IdOperarioComision { get; set; }
        public int idOperario { get; set; }
        public decimal Porcentaje { get; set; }
        public int DiaSemana { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
