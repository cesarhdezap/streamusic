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
        public byte[] DescargarArchivoPorId(string idArchivo)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLDescargaDeCanciones);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", idArchivo);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            byte[] archivo;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                archivo = JsonConvert.DeserializeObject<byte[]>(json);
            }
            else
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return archivo;
        }
    }
}
