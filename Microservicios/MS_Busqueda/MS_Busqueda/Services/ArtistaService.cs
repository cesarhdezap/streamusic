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
    public class ArtistaService
    {
        HttpClient cliente = new HttpClient();
        public ArtistaService()
        {
            cliente.BaseAddress = new Uri(DatosDeServicio.URLMSArtista);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Artista>> GetAllArtistasAsync()
        {
            List<Artista> artistas = null;
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(jsonString);
            }
            return artistas;
        }

        public async Task<List<Artista>> GetArtistasPorIdCancion(string idCancion)
        {
            CancionService cancionService = new CancionService();
            Cancion cancion = await cancionService.GetCancionPorId(idCancion);

            List<Artista> artistas = null;
            if (cancion != null)
            {
                artistas = new List<Artista>();
                foreach (string idArtista in cancion.IdsArtistas)
                {
                    Artista artista = await GetArtistaPorId(idArtista);
                    if(artista != null)
                    {
                        artistas.Add(artista);
                    }
                }
            }
            return artistas;
        }

        public async Task<Artista> GetArtistaPorNombre(string nombreArtista)
        {
            List<Artista> artistas = null;
            Artista resultado = new Artista();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(jsonString);
                resultado = artistas.Find(artista => artista.Nombre == nombreArtista);
            }

            return resultado;
        }


        public async Task<Artista> GetArtistaPorId(string id)
        {
            List<Artista> artistas = null;
            Artista resultado = new Artista();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                artistas = JsonConvert.DeserializeObject<List<Artista>>(jsonString);
                resultado = artistas.Find(artista => artista.Id == id);
            }

            return resultado;
        }
    }
}
