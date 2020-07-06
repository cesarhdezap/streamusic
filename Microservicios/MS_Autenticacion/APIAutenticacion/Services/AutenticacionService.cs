using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIAutenticacion.LogicaDeNegocio;
using APIAutenticacion.LogicaDeNegocio.Clases;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace APIAutenticacion
{
    public class AutenticacionService : SevicioAutenticacion.SevicioAutenticacionBase
    {
        private readonly ILogger<AutenticacionService> _logger;
        public AutenticacionService(ILogger<AutenticacionService> logger)
        {
            _logger = logger;
        }

        public override Task<LoginStatus> AutenticarUsuario(UsuarioAAutenticar request, ServerCallContext context)
        {
            Autenticacion autenticacion = new Autenticacion();
            Usuario usuario = autenticacion.AutenticarUsuario(request.Nombre, request.Password);

            LoginStatus status = new LoginStatus();
            if (usuario != null)
            {
                status.Codigo = 0;
                status.Mensaje = "Autenticacion exitosa.";
                status.UsuarioDelServicio = new UsuarioDelServicio();
                status.UsuarioDelServicio.Id = usuario.Id;
                status.UsuarioDelServicio.Contrasena = usuario.GetContrasena();
                status.UsuarioDelServicio.NombreDeUsuario = usuario.NombreDeUsuario;
                status.UsuarioDelServicio.TieneSuscripcion = usuario.TieneSuscripcion;
                if(usuario.IdArtista != null)
                {
                    status.UsuarioDelServicio.IdArtista = usuario.IdArtista;
                }
            }
            else
            {
                status.Codigo = 1;
                status.Mensaje = "Autenticacion fallida, las credenciales ingresadas no coinciden con alguna registrada en nuestro sistema.";
            }

            return Task.FromResult(status);
        }
    }
}