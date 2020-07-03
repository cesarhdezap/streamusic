using Logica.Clases;
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
        public List<ListaDeReproduccion> ObtenerTodasLasListasPorIdUsuario(string idUsuario)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLHistorialDeReproduccion + Urls.URLHistorialesDeReproduccionPorIdUsuario);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idUsuario", idUsuario);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<ListaDeReproduccion> listas = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                listas = JsonConvert.DeserializeObject<List<ListaDeReproduccion>>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return listas;
        }

        public bool CrearNuevaListaDeReproduccion(ListaDeReproduccion lista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLHistorialDeReproduccion);

            var metadatosJson = JsonConvert.SerializeObject(lista);
            var data = new StringContent(metadatosJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = Cliente.PostAsync(uriBuilder.Uri, data).Result;

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }

            return resultado;
        }
    }
}
