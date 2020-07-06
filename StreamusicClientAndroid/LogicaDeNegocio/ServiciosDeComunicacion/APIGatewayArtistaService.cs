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

            HttpResponseMessage respuesta = null;
            try
            {
                respuesta = Cliente.PostAsync(uriBuilder.Uri, data).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }
            

            string idArtista = null;

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                idArtista = JsonConvert.DeserializeObject<Artista>(json).Id;
                
            }

            return idArtista;
        }

        public bool ActualizarArtistaAsync(string id, Artista artista)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLArtista);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", id);
            uri.Query = nameValueCollection.ToString();

            var artistaJson = JsonConvert.SerializeObject(artista);
            var data = new StringContent(artistaJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = Cliente.PutAsync(uri.Uri, data).Result;
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

        public bool BorrarArtistaAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLArtista);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idArtista", id);
            uri.Query = nameValueCollection.ToString();

            
            HttpResponseMessage respuesa = null;
            try
            {
                respuesa = Cliente.DeleteAsync(uri.Uri).Result;
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
