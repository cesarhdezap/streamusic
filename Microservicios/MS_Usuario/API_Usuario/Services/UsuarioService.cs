using MongoDB.Driver;
using MS_Usuario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Usuario.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> Usuarios;

        public UsuarioService(IUsuarioDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Usuarios = database.GetCollection<Usuario>(settings.UsuariosCollectionName);
        }

        public List<Usuario> Get()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                usuarios = Usuarios.Find(usuario => true).ToList();
            }
            catch (TimeoutException e)
            {
                usuarios.Add(new Usuario()
                {
                    Id = "12",
                    NombreDeUsuario = "DatabaseCaida",
                    Contraseña = "pass",
                    TieneSuscripcion = true
                });
            }
            return usuarios;
        }


        public Usuario Get(string id) =>
            Usuarios.Find<Usuario>(usuario => usuario.Id == id).FirstOrDefault();

        public Usuario Create(Usuario usuario)
        {
            usuario.Id = string.Empty;
            Usuarios.InsertOne(usuario);
            return usuario;
        }

        public void Update(string id, Usuario usuarioActualizar) =>
            Usuarios.ReplaceOne(usuario => usuario.Id == id, usuarioActualizar);

        public void Remove(Usuario usuarioActualizar) =>
            Usuarios.DeleteOne(usuario => usuario.Id == usuarioActualizar.Id);

        public void Remove(string id) =>
            Usuarios.DeleteOne(usuario => usuario.Id == id);
    }
}