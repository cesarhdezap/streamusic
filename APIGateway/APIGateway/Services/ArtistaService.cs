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
    public class ArtistaService
    {
        HttpClient Cliente = new HttpClient();
        public ArtistaService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSArtista);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Artista> Crear(Artista artista)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);
            var artistaJson = JsonConvert.SerializeObject(artista);
            var data = new StringContent(artistaJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = await Cliente.PostAsync(uri.Uri, data);

            Artista artistaNuevo = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var jsonString = respuesa.Content.ReadAsStringAsync().Result;
                artistaNuevo = JsonConvert.DeserializeObject<Artista>(jsonString);
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return artistaNuevo;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return artistaNuevo;
        }

        public async Task<bool> ActualizarAsync(string id, Artista artista)
        {

            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/" + id);
            var artistaJson = JsonConvert.SerializeObject(artista);
            var data = new StringContent(artistaJson, Encoding.UTF8, "application/json");
            Console.WriteLine(uri.Uri.ToString());

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
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/" + id);

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
