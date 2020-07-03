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
        public bool CrearCancion(Cancion cancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLCancion);
            var cancionJson = JsonConvert.SerializeObject(cancion);
            var data = new StringContent(cancionJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = Cliente.PostAsync(uriBuilder.Uri, data).Result;

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }

            return resultado;
        }

        public async Task<bool> ActualizarCancionAsync(string id, Cancion cancion)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLCancion);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", id);
            uri.Query = nameValueCollection.ToString();

            var cancionJson = JsonConvert.SerializeObject(cancion);
            var data = new StringContent(cancionJson, Encoding.UTF8, "application/json");

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

        public async Task<bool> BorrarCancionAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLCancion);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", id);
            uri.Query = nameValueCollection.ToString();

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
