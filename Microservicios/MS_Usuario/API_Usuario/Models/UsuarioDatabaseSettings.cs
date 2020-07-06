using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Usuario.Models
{
    public class UsuarioDatabaseSettings : IUsuarioDatabaseSettings
    {
        public string UsuariosCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IUsuarioDatabaseSettings
    {
        string UsuariosCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
