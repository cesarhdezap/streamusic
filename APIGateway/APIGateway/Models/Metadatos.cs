using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public class Metadatos
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string IdCancion { get; set; }
        public string IdConsumidor { get; set; }
        public double CantidadDeVecesEscuchada { get; set; }
        public DateTime FechaPrimeraEscucha { get; set; }
        public DateTime FechaUltimaEscucha { get; set; }
        public int Calificacion { get; set; }
        public bool MeGusta { get; set; }

        public void Añadir(Metadatos metadatos)
        {
            if (Id == null)
            {
                Id = metadatos.Id;
            }
            if (IdCancion == null)
            {
                IdCancion = metadatos.IdCancion;
            }
            if (IdConsumidor == null)
            {
                IdConsumidor = metadatos.IdConsumidor;
            }
            if (CantidadDeVecesEscuchada < 1)
            {
                CantidadDeVecesEscuchada = metadatos.CantidadDeVecesEscuchada;
            }
            if (FechaPrimeraEscucha == DateTime.MinValue)
            {
                FechaPrimeraEscucha = metadatos.FechaPrimeraEscucha;
            }
            if (FechaUltimaEscucha == DateTime.MinValue)
            {
                FechaUltimaEscucha = metadatos.FechaUltimaEscucha;
            }
            if (Calificacion == 0 && metadatos.Calificacion > 0)
            {
                Calificacion = metadatos.Calificacion;
            }

        }
    }
}
