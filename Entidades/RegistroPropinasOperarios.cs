using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("RegistroPropinasOperarios")]
    public class RegistroPropinasOperarios
    {
        [PrimaryKey, AutoIncrement, Column("idPropina")]
        public int idPropina { get; set; }
        [ForeignKey(typeof(Operarios))]
        public int idOperario { get; set; }
        public DateTime fechaRegistro { get; set; }
        public decimal valorPropina { get; set; }
        public string observaciones { get; set; } = string.Empty;
        [ForeignKey(typeof(CajaDiaria))]
        public int idCaja { get; set; }
        public bool isDelete { get; set; }
    }
}
