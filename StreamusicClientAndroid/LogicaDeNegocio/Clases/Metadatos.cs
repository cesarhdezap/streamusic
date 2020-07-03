using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Clases
{
    public class Metadatos
    {
        public string Id { get; set; }
        public string IdCancion { get; set; }
        public string IdConsumidor { get; set; }
        public double CantidadDeVecesEscuchada { get; set; }
        public DateTime FechaPrimeraEscucha { get; set; }
        public DateTime FechaUltimaEscucha { get; set; }
        public int Calificacion { get; set; }
        public bool MeGusta { get; set; }
    }
}
