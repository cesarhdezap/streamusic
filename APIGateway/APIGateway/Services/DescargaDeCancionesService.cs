using APIGateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace APIGateway.Services
{
    public class DescargaDeCancionesService
    {
        HttpClient Cliente = new HttpClient();

        public DescargaDeCancionesService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSDescargaDeCanciones);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<byte[]> DescargarCancion(string idCancion)
        {
            byte[] cancion = null;

            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);
            var query = HttpUtility.ParseQueryString(uri.Query);
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("id", idCancion);

            query.Add(nameValueCollection);
            uri.Query = query.ToString();

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);
            if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                cancion = JsonConvert.DeserializeObject<byte[]>(json);
            }
            else
            {
                Console.WriteLine("ErrorDescargaDeCancionesService: " + respuesa.Content.ReadAsStringAsync().Result);
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return cancion;
        }

    }
}
