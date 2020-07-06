using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text;
using APIGateway.Models;
using Newtonsoft.Json;

namespace APIGateway.Services
{
    public class CargaDeCancionesService
    {
        HttpClient Cliente = new HttpClient();

        public CargaDeCancionesService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSCargaDeCanciones);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> CargarCancion(byte[] cancion)
        {
            string idRecibida = null;
            var cancionJson = JsonConvert.SerializeObject(cancion);
            var data = new StringContent(cancionJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Cliente.PostAsync(Cliente.BaseAddress, data);


            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                idRecibida = JsonConvert.DeserializeObject<string>(jsonString);
            }

            return idRecibida;
        }
    }
}
