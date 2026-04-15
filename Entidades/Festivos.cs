using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("Festivos")]
    public class Festivos
    {
        [PrimaryKey, AutoIncrement, Column("IdFestivo")]
        public int IdFestivo { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
