using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace CarWash.Entidades
{
    [Table("Usuarios")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement, Column("idUser")]
        public int idUser { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
        [MaxLength(80)]
        public string Nombres { get; set; }
        [MaxLength(80)]
        public string Apellidos { get; set; }
        [MaxLength(150)]
        public string DisplayName { get; set; }
        [MaxLength(500)]
        public string Email { get; set; }
        public bool isDelete { get; set; }
        [ForeignKey(typeof(Perfil))]
        public int PerfilId { get; set; }
    }
}
