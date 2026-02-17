using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("TurnosDiarios")]
    public class TurnosDiarios
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Consecutivo { get; set; }
    }
}
