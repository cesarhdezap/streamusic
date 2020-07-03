using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Clases
{
    public class RespuestaLogin
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public Usuario Usuario { get; set; }
    }
}
