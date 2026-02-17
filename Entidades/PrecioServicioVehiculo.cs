using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("PrecioServicioVehiculo")]
    internal class PrecioServicioVehiculo
    {
        [PrimaryKey, AutoIncrement, Column("IdServicioVehiculo ")]
        public int IdServicioVehiculo { get; set; }
        [ForeignKey(typeof(Servicios))]
        public int idServicio { get; set; }
        [ForeignKey(typeof(TipoVehiculo))]
        public int IdTipoVehiculo { get; set; }
        public decimal Precio { get; set; }
    }
}
