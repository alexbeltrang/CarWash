using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("TipoVehiculo")]
    public class TipoVehiculo
    {
        [PrimaryKey, AutoIncrement, Column("idTipoVehiculo")]
        public int IdTipoVehiculo { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = false;
    }
}
