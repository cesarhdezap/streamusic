using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MS_Busqueda.Models
{
    public class DatosDeServicio
    {
        public static string URLMSArtista { get; set; }
        public static string URLMSAlbum { get; set; }
        public static string URLMSCancion { get; set; }

        public static void ConfigurarURLS(IConfiguration configuration)
        {
            URLMSArtista = configuration.GetSection(nameof(DatosDeServicio))[nameof(URLMSArtista)];
            URLMSAlbum = configuration.GetSection(nameof(DatosDeServicio))[nameof(URLMSAlbum)];
            URLMSCancion = configuration.GetSection(nameof(DatosDeServicio))[nameof(URLMSCancion)];
        }
    }
}
