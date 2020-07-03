using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Clases
{
    public class Artista
    {
        public string Id { get; set; }
        public byte[] Ilustracion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public List<Cancion> Canciones { get; set; }

        public Artista()
        {
            Canciones = new List<Cancion>();
        }

        public void CargarCanciones()
        {

        }
    }
}
