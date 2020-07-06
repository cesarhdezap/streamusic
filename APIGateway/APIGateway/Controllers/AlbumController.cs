using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("album/[action]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        AlbumService AlbumService;

        public AlbumController()
        {
            AlbumService = new AlbumService();
        }

        [HttpPost]
        [ActionName("")]
        public IActionResult Crear(Album album)
        {
            if (album == null)
            {
                return BadRequest("id no puede ser null");
            }

            Album resultado;
            try
            {
                resultado = AlbumService.Crear(album).Result;
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
        public IActionResult Actualizar(string id, Album album)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado;
            try
            {
                resultado = AlbumService.ActualizarAsync(id, album).Result;
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
                resultado = AlbumService.BorrarAsync(id).Result;
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
