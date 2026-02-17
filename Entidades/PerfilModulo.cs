using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    public class PerfilModulo
    {
        [PrimaryKey, AutoIncrement, Column("PerfilModuloId")]
        public int PerfilModuloId { get; set; }
        [ForeignKey(typeof(Modulo))]
        public int ModuloId { get; set; }
        [ForeignKey(typeof(Perfil))]
        public int PerfilId { get; set; }
        public bool isDeleted { get; set; }
    }
}
