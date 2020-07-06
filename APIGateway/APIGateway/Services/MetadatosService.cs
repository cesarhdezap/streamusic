using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using APIGateway.Models;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Services
{
    public class MetadatosService
    {
        HttpClient Cliente = new HttpClient();

        public MetadatosService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSMetadatos);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Metadatos> Crear(Metadatos metadatos)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/postMetadata");
            var metadatosJson = JsonConvert.SerializeObject(metadatos);
            var data = new StringContent(metadatosJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = await Cliente.PostAsync(uri.Uri, data);

            Metadatos meta = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var jsonString = respuesa.Content.ReadAsStringAsync().Result;
                meta = JsonConvert.DeserializeObject<Metadatos>(jsonString);
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return meta;
            }
            else
            {
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.ToString());
            }

            return meta;
        }

        public async Task<Metadatos> ObtenerPorIdAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/getMetadataId/" + id);

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            Metadatos metadatos = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<Metadatos>(json);
            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return metadatos;
        }

        public async Task<Metadatos> ObtenerPorIdCancionAsync(string idCancion)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/Cancion/" + idCancion);

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            Metadatos metadatos = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<Metadatos>(json);

            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return metadatos;
        }

        public async Task<Metadatos> ObtenerPorIdCancionYIdUsuario(string idCancion, string idUsuario)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/getMetadataAll/");

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            List<Metadatos> metadatos = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<List<Metadatos>>(json);

            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return metadatos.FirstOrDefault(m => m.IdCancion == idCancion && m.IdConsumidor == idUsuario);
        }

        public async Task<List<Metadatos>> ObtenerPorIdConsumidorAsync(string idConsumidor)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/Consumidor/" + idConsumidor);

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            List<Metadatos> metadatos = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<List<Metadatos>>(json);

            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return metadatos;
        }

        public async Task<List<Metadatos>> ObtenerMeGustaPorIdConsumidorAsync(string idConsumidor)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/megusta/" + idConsumidor);

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            List<Metadatos> metadatos = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<List<Metadatos>>(json);
            }
            else if (respuesa.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return metadatos;
        }

        public async Task<bool> ActualizarAsync(string id, Metadatos metadatos)
        {
            Metadatos metadatosRecuperados = ObtenerPorIdAsync(id).Result;
            metadatos.Añadir(metadatosRecuperados);

            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/putMetadataId/" + id);
            var metadatosJson = JsonConvert.SerializeObject(metadatos);
            var data = new StringContent(metadatosJson, Encoding.UTF8, "application/json");
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
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return resultado;
        }

        public async Task<bool> BorrarAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/deleteMetadataId/" + id);

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
                Console.WriteLine("Excepcion: " + respuesa.ToString());
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return resultado;
        }
    }
}
