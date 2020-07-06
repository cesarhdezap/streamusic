using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace APIAutenticacion.LogicaDeNegocio.Clases
{
    public class Usuario
    {
        public string Id { get; set; }
        public string NombreDeUsuario { get; set; }

        public string IdArtista { get; set; }
        public string Contraseña { get; set; }
        public bool TieneSuscripcion { get; set; }

        public string GetContrasena()
        {
            return Contraseña;
        }
    }
}
