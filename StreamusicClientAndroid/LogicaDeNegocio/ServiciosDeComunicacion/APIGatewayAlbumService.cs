using Logica.Clases;
using Logica.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logica.ServiciosDeComunicacion
{
    public partial class APIGatewayService
    {
        public bool CrearAlbum(Album album)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLAlbum);
            var albumJson = JsonConvert.SerializeObject(album);
            var data = new StringContent(albumJson, Encoding.UTF8, "application/json");


            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = Cliente.PostAsync(uriBuilder.Uri, data).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }

            return resultado;
        }

        public async Task<bool> ActualizarAlbumAsync(string id, Album album)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLAlbum);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idAlbum", id);
            uri.Query = nameValueCollection.ToString();

            var albumJson = JsonConvert.SerializeObject(album);
            var data = new StringContent(albumJson, Encoding.UTF8, "application/json");


            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = await Cliente.PutAsync(uri.Uri, data);
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }
            

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

        public async Task<bool> BorrarAlbumAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLAlbum);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idAlbum", id);
            uri.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = await Cliente.DeleteAsync(uri.Uri);
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }


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
