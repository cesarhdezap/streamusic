using MongoDB.Driver;
using MS_Artista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Artista.Services
{
    public class ArtistaService
    {
        private readonly IMongoCollection<Artista> _artistas;

        public ArtistaService(IArtistaDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _artistas = database.GetCollection<Artista>(settings.ArtistaCollectionName);
        }

        public List<Artista> Get() =>
            _artistas.Find(artista => true).ToList();

        public Artista Get(string id)
        {
            return _artistas.Find<Artista>(artista => artista.Id == id).FirstOrDefault();          
        }

        public Artista Create(Artista artista)
        {
            _artistas.InsertOne(artista);
            return artista;
        }

        public void Update(string id, Artista artistaIn) =>
            _artistas.ReplaceOne(artista => artista.Id == id, artistaIn);

        public void Remove(Artista artistaIn) =>
            _artistas.DeleteOne(artista => artista.Id == artistaIn.Id);

        public void Remove(string id) =>
            _artistas.DeleteOne(artista => artista.Id == id);
    }
}
