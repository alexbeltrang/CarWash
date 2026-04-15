using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.DTOs
{
    public class LiquidacionResultadoDTO
    {
        public int IdOperario { get; set; }
        public string NombreOperario { get; set; }
        public int TotalServicios { get; set; }
        public decimal TotalFacturado { get; set; }
        public decimal Comision { get; set; }
        public decimal Vales { get; set; }
        public decimal TotalPagar { get; set; }
    }
}
