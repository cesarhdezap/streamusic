using Logica.Clases;
using Logica.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;

namespace Logica.ServiciosDeComunicacion
{
    public partial class APIGatewayService
    {
        public RespuestaLogin AutenticarUsuario(string nombreDeUsuario, string contraseña)
        {
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
            nameValueCollection.Add("nombreDeUsuario", nombreDeUsuario);
            nameValueCollection.Add("contraseña", contraseña);

            UriBuilder uriBuilder = new UriBuilder(Cliente.BaseAddress + Urls.URLAutenticar);
            uriBuilder.Query = nameValueCollection.ToString();

            HttpResponseMessage respuesta = null;
            try
            {
                respuesta = Cliente.GetAsync(uriBuilder.Uri).Result;
            }
            catch (AggregateException)
            {
                throw new Exception("Error. No hay conexión con el servidor.");
            }
            

            RespuestaLogin respuestaLogin = null;
            if (respuesta.IsSuccessStatusCode)
            {
                var json = respuesta.Content.ReadAsStringAsync().Result;
                respuestaLogin = JsonConvert.DeserializeObject<RespuestaLogin>(json);
            }
            else if (respuesta.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception(respuesta.Content.ReadAsStringAsync().Result);
            }

            return respuestaLogin;
        }
    }
}
