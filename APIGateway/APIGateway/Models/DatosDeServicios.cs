using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public static class DatosDeServicios
    {
        public static string URLMSUsuario { get; set; }
        public static string URLMSAutenticacion { get; set; }
        public static string URLMSDescargaDeCanciones { get; set; }
        public static string URLMSCargaDeCanciones { get; set; }
        public static string URLMSBusqueda { get; set; }
        public static string URLMSHistorialDeReproduccion { get; set; }
        public static string URLMSMetadatos { get; set; }
        public static string URLMSAlbum { get; set; }
        public static string URLMSCancion { get; set; }
        public static string URLMSArtista { get; set; }


        public static void ConfigurarURLS(IConfiguration configuration)
        {
            URLMSUsuario = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSUsuario)];
            URLMSAutenticacion = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSAutenticacion)];
            URLMSDescargaDeCanciones = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSDescargaDeCanciones)];
            URLMSCargaDeCanciones = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSCargaDeCanciones)];
            URLMSBusqueda = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSBusqueda)];
            URLMSHistorialDeReproduccion = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSHistorialDeReproduccion)];
            URLMSMetadatos = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSMetadatos)];
            URLMSAlbum = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSAlbum)];
            URLMSArtista = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSArtista)];
            URLMSCancion = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSCancion)];
        }
    }
}
