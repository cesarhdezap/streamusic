using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_HistorialDeReproduccion.Models
{
    public class HistorialDeReproduccionDatabaseSettings : IHistorialDeReproduccionDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IHistorialDeReproduccionDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

}
