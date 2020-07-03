using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Web;
using Logica.Clases;
using Logica.Recursos;
using Newtonsoft.Json;

namespace Logica.ServiciosDeComunicacion
{
    public partial class APIGatewayService
    {
        public List<Cancion> ObtenerTodasLasCanciones(string nombreDeUsuario, string contraseña)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarCanciones);

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Cancion> canciones = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return canciones;
        }

        public List<Cancion> ObtenerCancionesPorIdAlbum(string idAlbum)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarCancionesPorIdAlbum);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idAlbum", idAlbum);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Cancion> canciones = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return canciones;
        }

        public Cancion ObtenerCancionPorId(string id)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarCancionPorId);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", id);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            Cancion cancion = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                cancion = JsonConvert.DeserializeObject<Cancion>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return cancion;
        }

        
    }
}
