using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Entidades
{
    public class ServicioComboDTO
    {
        public int idServicio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal precio { get; set; }
    }
}
