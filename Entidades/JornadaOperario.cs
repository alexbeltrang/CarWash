using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("JornadaOperario")]
    public class JornadaOperario
    {
        [PrimaryKey, AutoIncrement, Column("idJornada")]
        public int idJornada { get; set; }
        public DateTime HoraIngreso { get; set; }
        public DateTime HoraSalida { get; set; }
        public bool Estado { get; set; }
        [ForeignKey(typeof(CajaDiaria))]
        public int idCaja { get; set; }
        [ForeignKey(typeof(Operarios))] 
        public int idOperario { get; set; }
    }
}
