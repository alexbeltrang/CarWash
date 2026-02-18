using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.DTOs
{
    public class OperariosDTO
    {
        public int idOperario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string NombreCompleto
        {
            get { return $"{Nombres} {Apellidos}"; }
        }
    }
}
