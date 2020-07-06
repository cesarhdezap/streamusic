using APIAutenticacion;
using APIGateway.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway.Services
{
    public class RespuestaLogin
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set;}
        public Usuario Usuario { get; set; }
    }

    public class AutenticacionService
    {
        public async Task<RespuestaLogin> AutenticarUsuario(string nombreDeUsuario, string contraseña)
        {
            //GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
            //grpcChannelOptions.Credentials = ChannelCredentials.Insecure;

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress(DatosDeServicios.URLMSAutenticacion, new GrpcChannelOptions { HttpHandler = httpHandler, Credentials = ChannelCredentials.Insecure });
            var client = new SevicioAutenticacion.SevicioAutenticacionClient(channel);

            
            var respuesta = await client.AutenticarUsuarioAsync(new UsuarioAAutenticar() { Nombre = nombreDeUsuario, Password = contraseña });
            
            var resLogin = new RespuestaLogin
            {
                Mensaje = respuesta.Mensaje,
                Codigo = respuesta.Codigo,
            };

            if (respuesta.UsuarioDelServicio != null)
            {
                resLogin.Usuario = new Usuario
                {
                    Id = respuesta.UsuarioDelServicio.Id,
                    NombreDeUsuario = respuesta.UsuarioDelServicio.NombreDeUsuario,
                    Contraseña = respuesta.UsuarioDelServicio.Contrasena,
                    TieneSuscripcion = respuesta.UsuarioDelServicio.TieneSuscripcion,
                    IdArtista = respuesta.UsuarioDelServicio.IdArtista
                };
            }

            return resLogin;
        }
    }
}
