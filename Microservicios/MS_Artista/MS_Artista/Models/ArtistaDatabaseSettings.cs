﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Artista.Models
{
    public class ArtistaDatabaseSettings : IArtistaDatabaseSettings
    {
        public string ArtistaCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IArtistaDatabaseSettings
    {
        string ArtistaCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

}
