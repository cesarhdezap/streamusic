using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Busqueda.Models
{
    public class Artista
    {
        public string Id { get; set; }
        public byte[] Ilustracion {get; set;}
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
