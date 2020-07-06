using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Busqueda.Models
{
    public class Album
    {
        public string Id { get; set; }
        public string IdArtista { get; set; }
        public byte[] Ilustracion { get; set; }
        public string Nombre { get; set; }
        public int AñoDeLanzamiento { get; set; }
        public string CompañiaDiscografica { get; set; }

    }
}
