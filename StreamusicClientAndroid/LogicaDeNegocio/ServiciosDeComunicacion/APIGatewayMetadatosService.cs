using Logica.Clases;
using Logica.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Logica.ServiciosDeComunicacion
{
    public partial class APIGatewayService
    {
        public List<Cancion> ObtenerCancionesGustadasPorIdUsuario(string idUsuario)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLMetadatos + Urls.URLBuscarMetadatosGustadosPorIdConsumidor);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idConsumidor", idUsuario);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            List<Metadatos> metadatos = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<List<Metadatos>>(json);
            }

            List<Cancion> canciones = new List<Cancion>();
            if (metadatos != null)
            {
                foreach (Metadatos metadato in metadatos)
                {
                    var cancion = ObtenerCancionPorId(metadato.IdCancion);
                    if(cancion != null)
                    {
                        canciones.Add(cancion);
                    }
                }
            }

            return canciones;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCancion"></param>
        /// <returns><see cref="Metadatos"/> o null si no se encuentra</returns>
        public Metadatos CargarMetadatosPorIdCancion(string idCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLMetadatos + Urls.URLBuscarMetadatosPorIdCancion);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", idCancion);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            Metadatos metadatos = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<Metadatos>(json);
            }

            return metadatos;
        }

        public Metadatos CargarMetadatosPorIdCancionYIdUsuario(string idCancion, string idUsuario)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLMetadatos + Urls.URLMetadatoPorIdCancionYIdUsuario);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idCancion", idCancion);
            nameValueCollection.Add("idUsuario", idUsuario);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;

            Metadatos metadatos = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                metadatos = JsonConvert.DeserializeObject<Metadatos>(json);
            }

            return metadatos;
        }

        public bool ActualizarMetadatos(Metadatos metadatos)
        {

            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLMetadatos);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", metadatos.Id);
            uriBuilder.Query = nameValueCollection.ToString();

            var metadatosJson = JsonConvert.SerializeObject(metadatos);
            var data = new StringContent(metadatosJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = Cliente.PutAsync(uriBuilder.Uri, data).Result;

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

        public bool ActualizarMeGustaAMetadato(string idMetadato, bool meGusta)
        {
            Metadatos metadatoAActualizar = new Metadatos
            {
                MeGusta = meGusta,
                Id = idMetadato,
                FechaUltimaEscucha = DateTime.Now
            };
            return ActualizarMetadatos(metadatoAActualizar);
        }

        public bool CrearNuevoMetadato(string idUsuario, string idCancion)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLMetadatos);

            Metadatos metadatos = new Metadatos
            {
                IdConsumidor = idUsuario,
                IdCancion = idCancion,
                FechaPrimeraEscucha = DateTime.Now,
                FechaUltimaEscucha = DateTime.Now,
            };

            var metadatosJson = JsonConvert.SerializeObject(metadatos);
            var data = new StringContent(metadatosJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = Cliente.PostAsync(uriBuilder.Uri, data).Result;

            bool resultado = false;

            if (respuesa.IsSuccessStatusCode)
            {
                resultado = true;
            }

            return resultado;
        }
    }
}
