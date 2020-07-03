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

        public List<Artista> ObtenerArtistas()
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBuscarArtistas);

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Artista> artistas = new List<Artista>();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(json);
            }

            return artistas;
        }

        public Artista ObtenerArtistaPorId(string id)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBusquedaArtistaPorId);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", id);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            Artista artista = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                artista = JsonConvert.DeserializeObject<Artista>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return artista;
        }

        public List<Artista> ObtenerArtistasPorIdCancion(string idCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLBusqueda + Urls.URLBusquedaArtistaPorIdCancion);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", idCancion);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Artista> artista = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                artista = JsonConvert.DeserializeObject<List<Artista>>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return artista;
        }
    }
}
