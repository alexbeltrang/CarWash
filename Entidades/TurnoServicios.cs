using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("TurnoServicios")]
    public class TurnoServicios
    {
        public long idTunoServicios { get; set; }
        public int idServicios { get; set; }
        [ForeignKey(typeof(Turnos))]
        public int IdTurno { get; set; }
        public bool IsDeleted { get; set; }
    }
}
