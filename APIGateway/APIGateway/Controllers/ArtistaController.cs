using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("artista/[action]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        ArtistaService ArtistaService;

        public ArtistaController()
        {
            ArtistaService = new ArtistaService();
        }

        [HttpPost]
        [ActionName("")]
        public IActionResult Crear(Artista artista)
        {
            if (artista == null)
            {
                return BadRequest("id no puede ser null");
            }

            Artista resultado;
            try
            {
                resultado = ArtistaService.Crear(artista).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en UsuarioService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (resultado == null)
            {
                return Conflict("ArtistaService regreso null como resultado.");
            }

            return Ok(resultado);
        }

        [HttpPut]
        [ActionName("")]
        public IActionResult Actualizar(string id, Artista artista)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado;
            try
            {
                resultado = ArtistaService.ActualizarAsync(id, artista).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en ArtistaService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
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
                resultado = ArtistaService.BorrarAsync(id).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en ArtistaService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
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
