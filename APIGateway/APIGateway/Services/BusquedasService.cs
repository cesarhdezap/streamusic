using APIGateway.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace APIGateway.Services
{
    public partial class BusquedasService
    {
        HttpClient Cliente = new HttpClient();
        private const string RUTA_CANCIONES = "/canciones/";
        private const string RUTA_ALBUMES = "/albums/";
        private const string RUTA_ARTISTAS = "/artistas/";

        public BusquedasService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSBusqueda);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Cancion> GetAllCanciones()
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES);
            return ObtenerCanciones(uriBuilder);
        }

        public List<Cancion> GetCancionesPorNombre(string nombreCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES + "cancion");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "nombreCancion", nombreCancion }
            };

            return ObtenerCanciones(uriBuilder, nameValueCollection);
        }

        public List<Cancion> GetCancionesPorArtista(string nombreArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES + "artista");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "nombreArtista", nombreArtista }
            };

            return ObtenerCanciones(uriBuilder, nameValueCollection);
        }

        public List<Cancion> GetCancionesPorAlbum(string nombreAlbum)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES + "album");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "nombreAlbum", nombreAlbum }
            };

            return ObtenerCanciones(uriBuilder, nameValueCollection);
        }

        public List<Cancion> GetCancionesPorIdAlbum(string idAlbum)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES + "idAlbum");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "idAlbum", idAlbum }
            };

            return ObtenerCanciones(uriBuilder, nameValueCollection);
        }

        public List<Cancion> GetCancionesPorIdArtista(string idArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES + "idArtista");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "idArtista", idArtista }
            };

            return ObtenerCanciones(uriBuilder, nameValueCollection);
        }

        public Cancion GetCancionPorId(string id)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_CANCIONES + "id");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "id", id }
            };

            return ObtenerCancion(uriBuilder, nameValueCollection);
        }

        private Cancion ObtenerCancion(UriBuilder uriBuilder, NameValueCollection nameValueCollection = null)
        {
            if (nameValueCollection != null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query.Add(nameValueCollection);
                uriBuilder.Query = query.ToString();
            }

            HttpResponseMessage respuesa = Cliente.GetAsync(uriBuilder.Uri).Result;
            Cancion cancion = null;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                cancion = JsonConvert.DeserializeObject<Cancion>(json);
            }
            else if(respuesa.StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return cancion;
        }

        private List<Cancion> ObtenerCanciones(UriBuilder uriBuilder, NameValueCollection nameValueCollection = null)
        {
            if (nameValueCollection != null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query.Add(nameValueCollection);
                uriBuilder.Query = query.ToString();
            }

            HttpResponseMessage respuesa = Cliente.GetAsync(uriBuilder.Uri).Result;
            List<Cancion> canciones;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(json);
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return canciones;
        }


        public async Task<List<Artista>> GetAllArtistas()
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ARTISTAS);
            HttpResponseMessage response = await Cliente.GetAsync(uriBuilder.Uri);
            List<Artista> artistas = null;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(jsonString);
            }
            return artistas;
        }

        public List<Artista> GetArtistasPorIdCancion(string idCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ARTISTAS + "idCancion");
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", idCancion);
            uriBuilder.Query = nameValueCollection.ToString();

            return ObtenerArtistas(uriBuilder);

        }

        public List<Artista> GetArtistasPorNombre(string nombreArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ARTISTAS + "artista");

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("nombreArtista", nombreArtista);
            uriBuilder.Query = nameValueCollection.ToString();

            return ObtenerArtistas(uriBuilder);
        }

        public Artista GetArtistaPorId(string id)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ARTISTAS + "id");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "id", id }
            };

            return ObtenerArtista(uriBuilder, nameValueCollection).Result;
        }

        private async Task<Artista> ObtenerArtista(UriBuilder uriBuilder, NameValueCollection nameValueCollection = null)
        {
            if (nameValueCollection != null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query.Add(nameValueCollection);
                uriBuilder.Query = query.ToString();
            }

            HttpResponseMessage respuesa = await Cliente.GetAsync(uriBuilder.Uri);
            Artista artista = null;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                artista = JsonConvert.DeserializeObject<Artista>(json);
            }
            else if (respuesa.StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return artista;
        }

        private List<Artista> ObtenerArtistas(UriBuilder uriBuilder)
        {
            Console.WriteLine(uriBuilder.Uri.ToString());
            HttpResponseMessage respuesa = Cliente.GetAsync(uriBuilder.Uri).Result;
            List<Artista> artistas = null;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(json);
            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return artistas;
        }


        public async Task<List<Album>> GetAllAlbums()
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ALBUMES);
            HttpResponseMessage response = await Cliente.GetAsync(uriBuilder.Uri);
            List<Album> albums = null;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                albums = JsonConvert.DeserializeObject<List<Album>>(jsonString);
            }
            return albums;
        }

        public Album GetAlbumPorIdCancion(string idCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ALBUMES + "idcancion");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "idCancion", idCancion }
            };

            return ObtenerAlbum(uriBuilder, nameValueCollection);
        }

        public List<Album> GetAlbumsPorNombre(string nombreAlbum)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ALBUMES + "album");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "nombreAlbum", nombreAlbum }
            };

            return ObtenerAlbums(uriBuilder, nameValueCollection);
        }

        public List<Album> GetAlbumsPorArtista(string nombreArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ALBUMES + "artista");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "nombreArtista", nombreArtista }
            };

            return ObtenerAlbums(uriBuilder, nameValueCollection);
        }

        public List<Album> GetAlbumsPorIdArtista(string idArtista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ALBUMES + "idArtista");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "idArtista", idArtista }
            };

            return ObtenerAlbums(uriBuilder, nameValueCollection);
        }

        public Album GetAlbumPorId(string id)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress.ToString() + RUTA_ALBUMES + "id");
            NameValueCollection nameValueCollection = new NameValueCollection
            {
                { "id", id }
            };

            return ObtenerAlbum(uriBuilder, nameValueCollection);
        }

        public Album ObtenerAlbum(UriBuilder uriBuilder, NameValueCollection nameValueCollection = null)
        {
            if (nameValueCollection != null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query.Add(nameValueCollection);
                uriBuilder.Query = query.ToString();
            }

            HttpResponseMessage respuesa = Cliente.GetAsync(uriBuilder.Uri).Result;
            Album album;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                album = JsonConvert.DeserializeObject<Album>(json);
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return album;
        }

        public List<Album> ObtenerAlbums(UriBuilder uriBuilder, NameValueCollection nameValueCollection = null)
        {
            if (nameValueCollection != null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query.Add(nameValueCollection);
                uriBuilder.Query = query.ToString();
            }

            HttpResponseMessage respuesa = Cliente.GetAsync(uriBuilder.Uri).Result;
            Console.WriteLine($"URI: {uriBuilder.Uri.ToString()}, respuesta codigo: {respuesa.StatusCode.ToString()}, respuesta request message: {respuesa.RequestMessage}, content: {respuesa.Content.ReadAsStringAsync()}");
            List<Album> albums = null;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                albums = JsonConvert.DeserializeObject<List<Album>>(json);
            }
            else if (respuesa.StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return albums;
        }
    }
}
