using API_Cancion.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cancion.Services
{
    public class CancionService
    {
        private readonly IMongoCollection<Cancion> Canciones;

        public CancionService(ICancionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Canciones = database.GetCollection<Cancion>(settings.CancionesCollectionName);
        }

        public List<Cancion> Get()
        {
            List<Cancion> usuarios = new List<Cancion>();
            try
            {
                usuarios = Canciones.Find(usuario => true).ToList();
            }
            catch (TimeoutException e)
            {
                usuarios.Add(new Cancion()
                {
                    Nombre = "Sin acceso a la base de datos."
                });
            }
            return usuarios;
        }


        public Cancion Get(string id) =>
            Canciones.Find<Cancion>(cancion => cancion.Id == id).FirstOrDefault();

        public Cancion Create(Cancion cancion)
        {
            cancion.Id = string.Empty;
            Canciones.InsertOne(cancion);
            return cancion;
        }

        public void Update(string id, Cancion cancionActualizar) =>
            Canciones.ReplaceOne(usuario => usuario.Id == id, cancionActualizar);

        public void Remove(Cancion cancionActualizar) =>
            Canciones.DeleteOne(cancion => cancion.Id == cancionActualizar.Id);

        public void Remove(string id) =>
            Canciones.DeleteOne(cancion => cancion.Id == id);
    }
}