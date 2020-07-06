using APIGateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Services
{
    public class AlbumService
    {
        HttpClient Cliente = new HttpClient();
        public AlbumService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSAlbum);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Album> Crear(Album album)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);
            var albumJson = JsonConvert.SerializeObject(album);
            var data = new StringContent(albumJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = await Cliente.PostAsync(uri.Uri, data);

            Album albumNuevo = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var jsonString = respuesa.Content.ReadAsStringAsync().Result;
                albumNuevo = JsonConvert.DeserializeObject<Album>(jsonString);
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return albumNuevo;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return albumNuevo;
        }

        public async Task<bool> ActualizarAsync(string id, Album album)
        {

            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/"+ id);
            var albumJson = JsonConvert.SerializeObject(album);
            var data = new StringContent(albumJson, Encoding.UTF8, "application/json");
            HttpResponseMessage respuesa = await Cliente.PutAsync(uri.Uri, data);

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return resultado;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return resultado;
        }

        public async Task<bool> BorrarAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/"+ id);

            HttpResponseMessage respuesa = await Cliente.DeleteAsync(uri.Uri);

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return resultado;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return resultado;
        }
    }
}
