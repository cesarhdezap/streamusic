using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_CargaDeCanciones.Models
{
    public static class DatosDeServicios
    {
        public static string URLMSArchivo { get; set; }


        public static void ConfigurarURLS(IConfiguration configuration)
        {
            URLMSArchivo = configuration.GetSection(nameof(DatosDeServicios))[nameof(URLMSArchivo)];
        }
    }
}
