using MS_Busqueda.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MS_Busqueda.Services
{
    public class AlbumService
    {
        HttpClient cliente = new HttpClient();
        HttpClient clienteArtista = new HttpClient();
        public AlbumService()
        {
            cliente.BaseAddress = new Uri(DatosDeServicio.URLMSAlbum);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            clienteArtista.BaseAddress = new Uri(DatosDeServicio.URLMSArtista);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<List<Album>> GetAllAlbumsAsync()
        {
            List<Album> albums = null;
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                albums = JsonConvert.DeserializeObject<List<Album>>(jsonString);
            }
            return albums;
        }

        public async Task<List<Album>> GetAlbumsPorNombre(string nombreAlbum)
        {
            List<Album> albums = null;
            List<Album> resultado = new List<Album>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                albums = JsonConvert.DeserializeObject<List<Album>>(jsonString);
                resultado.Add(albums.Find(album => album.Nombre == nombreAlbum));
            }

            return resultado;
        }

        public async Task<Album> GetAlbumPorId(string id)
        {
            List<Album> albums = null;
            Album resultado = new Album();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                albums = JsonConvert.DeserializeObject<List<Album>>(jsonString);
                resultado = albums.Find(album => album.Id == id);
            }

            return resultado;
        }

        public async Task<List<Album>> GetAlbumsPorIdArtista(string idArtista)
        {
            List<Album> albums = null;
            List<Album> resultado = new List<Album>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                albums = JsonConvert.DeserializeObject<List<Album>>(jsonString);
            }

            foreach(Album album in albums)
            {
                if(album.IdArtista == idArtista)
                {
                    resultado.Add(album);
                }
            }

            return resultado;
        }

        public async Task<Album> GetAlbumPorIdCancion(string idCancion)
        {
            CancionService cancionService = new CancionService();
            Cancion cancion = await cancionService.GetCancionPorId(idCancion);

            Album album = null;
            if (cancion != null)
            {
                album = await GetAlbumPorId(cancion.IdAlbum);
            }
            return album;
        }


        public async Task<List<Album>> GetAlbumsPorArtista(string nombreArtista)
        {
            List<Artista> artistas = null;
            List<Artista> resultadoArtistas = new List<Artista>();
            List<Album> resultado = null;
            HttpResponseMessage responseArtista = await clienteArtista.GetAsync(clienteArtista.BaseAddress);
            if (responseArtista.IsSuccessStatusCode)
            {
                var jsonString = responseArtista.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(jsonString);

                if (artistas.Exists(artista => artista.Nombre == nombreArtista))
                {
                    Artista artista = artistas.FirstOrDefault(a => a.Nombre == nombreArtista);

                    List<Album> albums = null;
                    HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStringAlbum = response.Content.ReadAsStringAsync().Result;
                        albums = JsonConvert.DeserializeObject<List<Album>>(jsonStringAlbum);
                        resultado = new List<Album>();
                        resultado.Add(albums.Find(album => album.IdArtista == artista.Id));
                    }
                }
            }
            return resultado;
        }

    }
}
