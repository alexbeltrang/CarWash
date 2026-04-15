using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("AsistenciaOperario")]
    public class AsistenciaOperario
    {
        [PrimaryKey, AutoIncrement, Column("idAsistenciaOperario")]
        public int idAsistenciaOperario { get; set; }
        [ForeignKey(typeof(Operarios))]
        public int idOperario { get; set; }
        public DateTime Fecha { get; set; }
        public bool Asistio { get; set; }
        public bool Autorizado { get; set; }
        public string Observacion { get; set; }
    }
}
