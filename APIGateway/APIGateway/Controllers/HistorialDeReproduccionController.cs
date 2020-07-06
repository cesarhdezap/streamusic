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
    [Route("historialdereproduccion/[action]")]
    [ApiController]
    public class HistorialDeReproduccionController : ControllerBase
    {
        HistorialDeReproduccionService Service;

        public HistorialDeReproduccionController()
        {
            Service = new HistorialDeReproduccionService();
        }

        [HttpGet]
        [ActionName("")]
        public IActionResult GetPorId(string id)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            ListaDeReproduccion canciones;
            try
            {
                canciones = Service.ObtenerPorIdAsync(id).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en HistorialDeReproduccionService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("poridusuario")]
        public IActionResult GetPorIdUsuario(string idUsuario)
        {
            if (idUsuario == null)
            {
                return BadRequest("idUsuario no puede ser null");
            }

            List<ListaDeReproduccion> listas;
            try
            {
                listas = Service.ObtenerTodos(idUsuario).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en HistorialDeReproduccionService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (listas == null)
            {
                return NotFound();
            }

            List<ListaDeReproduccion> listasPorUsuario = listas.Where(l => l.IdUsuario == idUsuario).ToList();
            if (listasPorUsuario.Count <= 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(listasPorUsuario);
            }
        }

        [HttpPost]
        [ActionName("")]
        public IActionResult Crear(ListaDeReproduccion listaDeReproduccion)
        {
            if (listaDeReproduccion == null)
            {
                return BadRequest("id no puede ser null");
            }

            ListaDeReproduccion resultado;
            try
            {
                resultado = Service.Crear(listaDeReproduccion).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en HistorialDeReproduccionService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        [HttpPut]
        [ActionName("")]
        public IActionResult Actualizar(string id, ListaDeReproduccion listaDeReproduccion)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado;
            try
            {
                resultado = Service.ActualizarAsync(id, listaDeReproduccion).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en HistorialDeReproduccionService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
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
                resultado = Service.BorrarAsync(id).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en HistorialDeReproduccionService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
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
