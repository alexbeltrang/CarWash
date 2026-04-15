using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.DTOs
{
    public class ServicioNominaDTO
    {
        public int IdOperario { get; set; }
        public long FechaHoraIngreso { get; set; }
        public decimal ValorServicio { get; set; }
        public DateTime Fecha => new DateTime(FechaHoraIngreso);
    }

}
