using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_HistorialDeReproduccion.Models
{
    public class ListaDeReproduccion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> IdsCanciones { get; set; }
        public string IdUsuario { get; set; }
        public bool EsHistorialDeReproduccion { get; set; }
    }
}
