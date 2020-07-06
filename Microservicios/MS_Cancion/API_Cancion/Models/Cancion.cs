using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cancion.Models
{
    public class Cancion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string IdAlbum { get; set; }
        public List<string> IdsArtistas { get; set; }
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public int Calificacion { get; set; }
        public int VecesEscuchada { get; set; }
        public double Duracion { get; set; }
        public string IdArchivo { get; set; }
        public bool EsPublico {get; set;}
    }
}
