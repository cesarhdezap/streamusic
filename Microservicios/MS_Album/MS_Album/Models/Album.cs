using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Album.Models
{
    public class Album
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IdArtista { get; set; }

        public byte[] Ilustracion { get; set; }

        public string Nombre { get; set; }

        public int AñoDeLanzamiento { get; set; }

        public string CompañiaDiscografica { get; set; }

    }
}
