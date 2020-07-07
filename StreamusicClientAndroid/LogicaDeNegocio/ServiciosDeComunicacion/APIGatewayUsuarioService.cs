using Logica;
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
        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLUsuario);

            HttpResponseMessage respuesta = null;
            try
            {
                respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }

            List<Usuario> usuarios = new List<Usuario>();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
            }

            return usuarios;
        }

        public bool CrearUsuario(Usuario usuario)
        {
            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLUsuario);
            var usuarioJson = JsonConvert.SerializeObject(usuario);
            var data = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

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

        public bool ActualizarUsuarioAsync(string id, Usuario usuario)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLUsuario );
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("id", id);
            uri.Query = nameValueCollection.ToString();

            var usuarioJson = JsonConvert.SerializeObject(usuario);
            var data = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

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
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return resultado;
        }

        public async Task<bool> BorrarUsuarioAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + Urls.URLUsuario);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("idUsuario", id);
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
