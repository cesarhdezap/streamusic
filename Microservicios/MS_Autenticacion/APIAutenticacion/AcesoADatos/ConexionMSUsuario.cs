using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using APIAutenticacion.LogicaDeNegocio.Clases;
using Newtonsoft.Json;

namespace APIAutenticacion.AcesoADatos
{
    public class ConexionMSUsuario
    {
        HttpClient cliente = new HttpClient();

        public  void Conectar()
        {
            cliente.BaseAddress = new Uri("http://ms_usuario:80/api/usuarios");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

         public  async Task<List<Usuario>> GetUsuariosAsync()
        {
            List<Usuario> usuario  = null;
            HttpResponseMessage response = await cliente.GetAsync(cliente.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                usuario = await response.Content.ReadAsAsync<List<Usuario>>();
            }
            
            return usuario;
        }

    }
}
