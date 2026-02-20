using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.DTOs
{
    public class DashboardOperadoresDTO
    {
        public string NombreOperador { get; set; }
        public string Placa { get; set; }
        public DateTime FechaInicio { get; set; }
        public int MinutosTranscurridos { get; set; }

        public string TiempoFormateado
        {
            get
            {
                TimeSpan ts = TimeSpan.FromMinutes(MinutosTranscurridos);
                return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}";
            }
        }
    }
}
