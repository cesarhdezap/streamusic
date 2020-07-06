using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string IdArtista {get; set;}
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public bool TieneSuscripcion { get; set; }
    }
}
