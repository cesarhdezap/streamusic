using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Resources;
using System.Reflection;
using System.Globalization;
using Logica.Recursos;

namespace Logica.ServiciosDeComunicacion
{
    public partial class APIGatewayService
    {
        HttpClient Cliente = new HttpClient();
        public APIGatewayService()
        {   
            Cliente.BaseAddress = new Uri(Urls.URLAPIGateway);
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
