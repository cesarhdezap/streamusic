using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("usuario/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        UsuarioService UsuarioService;

        public UsuarioController()
        {
            UsuarioService = new UsuarioService();
        }

        [HttpGet]
        [ActionName("")]
        public IActionResult ObtenerUsuarios()
        {
            List<Usuario> resultado;
            try
            {
                resultado = UsuarioService.ObtenerUsuarios().Result;
            }catch(Exception e)
            {
                return Conflict("Error en UsuarioService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if(resultado == null)
            {
                return Conflict("MicroServicioUsuario regreso null como resultado.");
            }

            return Ok(resultado);

        }

        [HttpPost]
        [ActionName("")]
        public IActionResult Crear(Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("id no puede ser null");
            }

            Usuario resultado;
            try
            {
                resultado = UsuarioService.Crear(usuario).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en UsuarioService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (resultado == null)
            {
                return Conflict("MicroServicioUsuario regreso null como resultado.");
            }

            return Ok(resultado);
        }

        [HttpPut]
        [ActionName("")]
        public IActionResult Actualizar(string id, Usuario usuario)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado;
            try
            {
                resultado = UsuarioService.ActualizarAsync(id, usuario).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en UsuarioService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (!resultado)
            {
                return Conflict("No se ha podido actualizar el usuario.");
            }
            else
            {
                return Ok();
            }
        }

        [HttpDelete]
        [ActionName("")]
        public IActionResult Eliminar(string id)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado = false;
            try
            {
                resultado = UsuarioService.BorrarAsync(id).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en UsuarioService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (!resultado)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
    }
}
