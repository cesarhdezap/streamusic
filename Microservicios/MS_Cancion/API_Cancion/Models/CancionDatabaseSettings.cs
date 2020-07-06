using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Cancion.Models
{
    public class CancionDatabaseSettings : ICancionDatabaseSettings
    {
        public string CancionesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICancionDatabaseSettings
    {
        string CancionesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
