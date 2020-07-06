﻿using APIGateway.Models;
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
    public class CancionService
    {
        HttpClient Cliente = new HttpClient();
        public CancionService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSCancion);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Cancion> Crear(Cancion cancion)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);
            var cancionJson = JsonConvert.SerializeObject(cancion);
            var data = new StringContent(cancionJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = await Cliente.PostAsync(uri.Uri, data);

            Cancion cancionNuevo = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var jsonString = respuesa.Content.ReadAsStringAsync().Result;
                cancionNuevo = JsonConvert.DeserializeObject<Cancion>(jsonString);
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return cancionNuevo;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return cancionNuevo;
        }

        public async Task<bool> ActualizarAsync(string id, Cancion cancion)
        {

            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + id);
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

        public async Task<bool> BorrarAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + id);

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
