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

            HttpResponseMessage respuesta = null;
            try
            {
                respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }

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

            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = Cliente.PostAsync(uriBuilder.Uri, data).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }

            return resultado;
        }

        public bool ActualizarListaDeReproduccion(string id, ListaDeReproduccion lista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLHistorialDeReproduccion);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", id);
            uriBuilder.Query = nameValueCollection.ToString();

            var listaJson = JsonConvert.SerializeObject(lista);
            var data = new StringContent(listaJson, Encoding.UTF8, "application/json");
            
            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = Cliente.PutAsync(uriBuilder.Uri, data).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }

            return resultado;
        }


    }
}
