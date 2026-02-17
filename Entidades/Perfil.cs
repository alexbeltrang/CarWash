using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("Perfiles")]
    public class Perfil
    {
        [PrimaryKey, AutoIncrement, Column("PerfilId")]
        public int PerfilId { get; set; }
        [MaxLength(150)]
        public string NombrePerfil { get; set; }
        public bool isDelete { get; set; }

    }
}
