using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("Servicios")]
    public class Servicios
    {
        [PrimaryKey, AutoIncrement, Column("idServicio")]
        public int idServicio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = false;
    }
}
