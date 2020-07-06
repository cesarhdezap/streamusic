using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("cancion/[action]")]
    [ApiController]
    public class CancionController : ControllerBase
    {
        CancionService CancionService;

        public CancionController()
        {
            CancionService = new CancionService();
        }

        [HttpPost]
        [ActionName("")]
        public IActionResult Crear(Cancion cancion)
        {
            if (cancion == null)
            {
                return BadRequest("id no puede ser null");
            }

            Cancion resultado;
            try
            {
                resultado = CancionService.Crear(cancion).Result;
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
        public IActionResult Actualizar(string id, Cancion cancion)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado;
            try
            {
                resultado = CancionService.ActualizarAsync(id, cancion).Result;
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
                resultado = CancionService.BorrarAsync(id).Result;
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
