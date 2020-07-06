using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Usuario.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IdArtista{get; set;}

        public string NombreDeUsuario { get; set; }

        public string Contraseña { get; set; }

        public bool TieneSuscripcion { get; set; }
    }
}
