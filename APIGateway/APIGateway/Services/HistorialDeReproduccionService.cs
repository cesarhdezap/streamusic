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
    public class HistorialDeReproduccionService
    {

        HttpClient Cliente = new HttpClient();

        public HistorialDeReproduccionService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSHistorialDeReproduccion);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ListaDeReproduccion> ObtenerPorIdAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/" + id);

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            ListaDeReproduccion listaDeReproduccion = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                listaDeReproduccion = JsonConvert.DeserializeObject<ListaDeReproduccion>(json);
            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return listaDeReproduccion;
        }

        public async Task<List<ListaDeReproduccion>> ObtenerTodos(string idUsuario)
        {
            HttpResponseMessage respuesa = await Cliente.GetAsync(Cliente.BaseAddress);

            List<ListaDeReproduccion> listas;
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                listas = JsonConvert.DeserializeObject<List<ListaDeReproduccion>>(json);
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return listas;
        }

        public async Task<bool> ActualizarAsync(string id, ListaDeReproduccion listaDeReproduccion)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/" + id);
            var listaJson = JsonConvert.SerializeObject(listaDeReproduccion);
            var data = new StringContent(listaJson, Encoding.UTF8, "application/json");
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
            else if(respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return resultado;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return resultado;
        }

        public async Task<ListaDeReproduccion> Crear(ListaDeReproduccion listaDeReproduccion)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);
            var listaJson = JsonConvert.SerializeObject(listaDeReproduccion);
            var data = new StringContent(listaJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = await Cliente.PostAsync(uri.Uri, data);

            ListaDeReproduccion lista = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var jsonString = respuesa.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<ListaDeReproduccion>(jsonString);
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return lista;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return lista;
        }
    }
}
