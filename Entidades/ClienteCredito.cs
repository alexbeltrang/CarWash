using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    [Table("ClienteCredito")]
    public class ClienteCredito
    {
        [PrimaryKey, AutoIncrement, Column("idClienteCredito")]
        public int idClienteCredito { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direcion { get; set; }
        public string Correo { get; set; }
        public string ciudad { get; set; }
        public bool Estado { get; set; }
    }
}
