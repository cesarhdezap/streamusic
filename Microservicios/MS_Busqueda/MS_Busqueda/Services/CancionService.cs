using MS_Busqueda.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MS_Busqueda.Services
{
    public class CancionService
    {
        HttpClient cliente = new HttpClient();
        public CancionService()
        {
            cliente.BaseAddress = new Uri(DatosDeServicio.URLMSCancion);
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Cancion>> GetAllCancionesAsync()
        {
            List<Cancion> canciones = null;
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);
            }
            return canciones;
        }


        public async Task<List<Cancion>> GetCancionesPorNombre(string nombreCancion)
        {
            List<Cancion> canciones = null;
            List<Cancion> resultado = new List<Cancion>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);
                resultado.Add(canciones.Find(cancion => cancion.Nombre == nombreCancion));
            }

            return resultado;
        }

        public async Task<Cancion> GetCancionPorId(string id)
        {
            List<Cancion> canciones = null;
            Cancion resultado = null;
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);
                resultado = canciones.Find(cancion => cancion.Id == id);
            }

            return resultado;
        }

        public async Task<List<Cancion>> GetCancionesPorArtista(string nombreArtista)
        {
            List<Cancion> canciones = null;
            List<Cancion> resultado = new List<Cancion>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);

                foreach (Cancion cancion in canciones)
                {
                    if(cancion.IdsArtistas.Exists(artista => nombreArtista == artista))
                    {
                        resultado.Add(cancion);
                    }
                }
            }

            return resultado;
        }
        public async Task<List<Cancion>> GetCancionesPorAlbum(string nombreAlbum)
        {
            List<Cancion> canciones = null;
            List<Cancion> resultado = new List<Cancion>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);
                resultado.Add(canciones.Find(cancion => cancion.IdAlbum == nombreAlbum));
            }

            return resultado;
        }

        public async Task<List<Cancion>> GetCancionesPorIdAlbum(string idAlbum)
        {
            List<Cancion> canciones = null;
            List<Cancion> resultado = new List<Cancion>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);
            }

            if(canciones != null)
            {
                foreach(Cancion cancion in canciones)
                {
                    if(cancion.IdAlbum == idAlbum)
                    {
                        resultado.Add(cancion);
                    }
                }
            }
            else
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<List<Cancion>> GetCancionesPorIdArtista(string idArtista)
        {
            List<Cancion> canciones = null;
            List<Cancion> resultado = new List<Cancion>();
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                canciones = JsonConvert.DeserializeObject<List<Cancion>>(jsonString);
            }

            if(canciones != null)
            {
                foreach(Cancion cancion in canciones)
                {
                    if(cancion.IdsArtistas.Contains(idArtista))
                    {
                        resultado.Add(cancion);
                    }
                }
            }
            else
            {
                resultado = null;
            }

            return resultado;
        }
    }
}
