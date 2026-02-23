using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("FormaPago")]
    public class FormaPago
    {
        [PrimaryKey, AutoIncrement, Column("IdFormaPago")]
        public int IdFormaPago { get; set; }
        public string Nombre { get; set; }
    }
}
