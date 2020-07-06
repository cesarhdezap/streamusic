using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Artista.Models
{
    public class Artista
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public byte[] Ilustracion {get; set;}
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }
}
