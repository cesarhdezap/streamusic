using APIGateway.Models;
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
    public class UsuarioService
    {
        HttpClient Cliente = new HttpClient();

        public UsuarioService()
        {
            Cliente.BaseAddress = new Uri(DatosDeServicios.URLMSUsuario);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Usuario>> ObtenerUsuarios()
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);

            HttpResponseMessage respuesa = await Cliente.GetAsync(uri.Uri);

            List<Usuario> usuarios = new List<Usuario>();

             if (respuesa.IsSuccessStatusCode)
            {
                var json = respuesa.Content.ReadAsStringAsync().Result;
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return usuarios;
            
        }

        public async Task<Usuario> Crear(Usuario usuario)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress);
            var usuarioJson = JsonConvert.SerializeObject(usuario);
            var data = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesa = await Cliente.PostAsync(uri.Uri, data);

            Usuario usuarioNuevo = null;

            if (respuesa.IsSuccessStatusCode)
            {
                var jsonString = respuesa.Content.ReadAsStringAsync().Result;
                usuarioNuevo = JsonConvert.DeserializeObject<Usuario>(jsonString);
            }
            else if (respuesa.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return usuarioNuevo;
            }
            else
            {
                throw new Exception(respuesa.Content.ReadAsStringAsync().Result);
            }

            return usuarioNuevo;
        }

        public async Task<bool> ActualizarAsync(string id, Usuario usuario)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/" + id);
            var usuarioJson = JsonConvert.SerializeObject(usuario);
            var data = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
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

            Console.WriteLine("ACTUALIZADO DE USUARIO TERMINADO");
            return resultado;
        }

        public async Task<bool> BorrarAsync(string id)
        {
            UriBuilder uri = new UriBuilder(Cliente.BaseAddress + "/" + id);

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
