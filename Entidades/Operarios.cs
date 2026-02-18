using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("Operarios")]
    public class Operarios
    {
        [PrimaryKey, AutoIncrement, Column("idOperario")]
        public int idOperario { get; set; }
        [MaxLength(80)]
        public string Nombres { get; set; }
        [MaxLength(80)]
        public string Apellidos { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Celular { get; set; }
        public bool isDelete { get; set; } = false;
        public DateTime UltimaAsignacion { get; set; }
        public int CargaTrabajo { get; set; }
    }
}
