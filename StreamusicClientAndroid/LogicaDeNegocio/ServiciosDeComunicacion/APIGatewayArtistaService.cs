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
        public string CrearArtista(Artista artista)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLArtista);
            var artistaJson = JsonConvert.SerializeObject(artista);
            var data = new StringContent(artistaJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta = Cliente.PostAsync(uriBuilder.Uri, data).Result;

            string idArtista = null;

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                idArtista = JsonConvert.DeserializeObject<Artista>(json).Id;
                
            }

            return idArtista;
        }

        public async Task<bool> ActualizarArtistaAsync(string id, Artista artista)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLArtista);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idArtista", id);
            uri.Query = nameValueCollection.ToString();

            var artistaJson = JsonConvert.SerializeObject(artista);
            var data = new StringContent(artistaJson, Encoding.UTF8, "application/json");

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

        public async Task<bool> BorrarArtistaAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLArtista);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idArtista", id);
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
