using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("Modulos")]
    public class Modulo
    {
        [PrimaryKey, AutoIncrement, Column("ModuloId")]
        public int ModuloId { get; set; }
        [MaxLength(150)]
        public string Descripcion { get; set; }
        public bool IsDelete { get; set; }
        public int ModuloPadre { get; set; }
        public int OrdenPresentacion { get; set; }
        [MaxLength(300)]
        public string NombreMenuApp { get; set; }
    }
}
