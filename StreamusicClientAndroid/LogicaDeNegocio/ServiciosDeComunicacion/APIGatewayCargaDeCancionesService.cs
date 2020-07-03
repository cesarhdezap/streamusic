using Logica.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Logica.ServiciosDeComunicacion
{
    public partial class APIGatewayService
    {
        public string CargarArchivos(byte[] archivo)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLCargaDeCanciones);

            var archivoJson = JsonConvert.SerializeObject(archivo);
            var data = new StringContent(archivoJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta = Cliente.PostAsync(uriBuilder.Uri, data).Result;

            string idArchivo;

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                idArchivo = JsonConvert.DeserializeObject<string>(json);
            }
            else
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return idArchivo;
        }
    }
}
