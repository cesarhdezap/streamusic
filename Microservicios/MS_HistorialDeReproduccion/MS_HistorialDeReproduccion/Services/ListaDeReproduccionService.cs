using MongoDB.Driver;
using MS_HistorialDeReproduccion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_HistorialDeReproduccion.Services
{
    public class ListaDeReproduccionService
    {
        private readonly IMongoCollection<ListaDeReproduccion> _listasDeReproduccion;

        public ListaDeReproduccionService(IHistorialDeReproduccionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _listasDeReproduccion = database.GetCollection<ListaDeReproduccion>(settings.CollectionName);
        }

        public List<ListaDeReproduccion> Get() =>
            _listasDeReproduccion.Find(listaDeReproduccion => true).ToList();

        public ListaDeReproduccion Get(string id) =>
            _listasDeReproduccion.Find<ListaDeReproduccion>(listaDeReproduccion => listaDeReproduccion.Id == id).FirstOrDefault();

        public ListaDeReproduccion Create(ListaDeReproduccion listaDeReproduccion)
        {
            _listasDeReproduccion.InsertOne(listaDeReproduccion);
            return listaDeReproduccion;
        }

        public void Update(string id, ListaDeReproduccion listaDeReproduccionIn)
        {
            listaDeReproduccionIn.Id = id;
            _listasDeReproduccion.ReplaceOne(listaDeReproduccion => listaDeReproduccion.Id == id, listaDeReproduccionIn);
        }

        public void Remove(ListaDeReproduccion listaDeReproduccionIn) =>
            _listasDeReproduccion.DeleteOne(listaDeReproduccion => listaDeReproduccion.Id == listaDeReproduccionIn.Id);

        public void Remove(string id) =>
            _listasDeReproduccion.DeleteOne(listaDeReproduccion => listaDeReproduccion.Id == id);

    }
}
