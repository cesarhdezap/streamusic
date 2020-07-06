using MongoDB.Driver;
using MS_Album.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Album.Services
{
    public class AlbumService
    {
        private readonly IMongoCollection<Album> _albumes;

        public AlbumService(IAlbumDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _albumes = database.GetCollection<Album>(settings.AlbumCollectionName);
        }

        public List<Album> Get() =>
            _albumes.Find(album => true).ToList();

        public Album Get(string id) =>
            _albumes.Find<Album>(album => album.Id == id).FirstOrDefault();

        public Album Create(Album album)
        {
            _albumes.InsertOne(album);
            return album;
        }

        public void Update(string id, Album albumIn) =>
            _albumes.ReplaceOne(album => album.Id == id, albumIn);

        public void Remove(Album albumIn) =>
            _albumes.DeleteOne(album => album.Id == albumIn.Id);

        public void Remove(string id) =>
            _albumes.DeleteOne(album => album.Id == id);
    }
}
