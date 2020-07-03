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
        public Album ObtenerAlbumPorIdCancion(string idCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBusquedaAlbumPorIdCancion);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", idCancion);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            Album artista = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                artista = JsonConvert.DeserializeObject<Album>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return artista;
        }

        public List<Album> ObtenerAlbumsPorIdArtista(string idArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarAlbumsPorIdArtista);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idArtista", idArtista);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Album> albumes = new List<Album>();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                albumes = JsonConvert.DeserializeObject<List<Album>>(json);
            }

            return albumes;
        }

        public  List<Album> ObtenerTodosLosAlbumes()
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLAlbum);

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Album> albumes = new List<Album>();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                albumes = JsonConvert.DeserializeObject<List<Album>>(json);
            }

            return albumes;
        }

        public List<Album> ObtenerAlbumsPorNombre(string nombre)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarAlbumPorNombre);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("nombre", nombre);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Album> albumes = new List<Album>();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                albumes = JsonConvert.DeserializeObject<List<Album>>(json);
            }

            return albumes;
        }
        public List<Album> ObtenerAlbumsPorArtista(string nombreArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarAlbumPorNombreArtista);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("nombreArtista", nombreArtista);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Album> albumes = new List<Album>();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                albumes = JsonConvert.DeserializeObject<List<Album>>(json);
            }

            return albumes;
        }
    }
}
