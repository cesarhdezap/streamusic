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
    [Route("metadatos/[action]")]
    [ApiController]
    public class MetadatosController : ControllerBase
    {
        MetadatosService MetadatosService;

        public MetadatosController()
        {
            MetadatosService = new MetadatosService();
        }

        [HttpGet]
        [ActionName("")]
        public IActionResult GetPorId(string id)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            Metadatos metadatos;
            try
            {
                metadatos = MetadatosService.ObtenerPorIdAsync(id).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (metadatos == null)
            {
                return NotFound();
            }

            return Ok(metadatos);
        }

        [HttpGet]
        [ActionName("poridcancion")]
        public IActionResult GetPorIdCancion(string idCancion)
        {
            if (idCancion == null)
            {
                return BadRequest("idCancion no puede ser null");
            }

            Metadatos metadatos;
            try
            {
                metadatos = MetadatosService.ObtenerPorIdCancionAsync(idCancion).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (metadatos == null)
            {
                return NotFound();
            }

            return Ok(metadatos);
        }

        [HttpGet]
        [ActionName("poridcancionyidusuario")]
        public IActionResult GetPorIdCancionYIdUsuario(string idCancion, string idUsuario)
        {
            if (idCancion == null)
            {
                return BadRequest("idCancion no puede ser null");
            }
            if (idUsuario == null)
            {
                return BadRequest("idUsuario no puede ser null");
            }

            Metadatos metadatos;
            try
            {
                metadatos = MetadatosService.ObtenerPorIdCancionYIdUsuario(idCancion, idUsuario).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (metadatos == null)
            {
                return NotFound();
            }

            return Ok(metadatos);
        }

        [HttpGet]
        [ActionName("poridconsumidor")]
        public IActionResult GetPorIdConsumidor(string idConsumidor)
        {
            if (idConsumidor == null)
            {
                return BadRequest("idConsumidor no puede ser null");
            }

            List<Metadatos> metadatos;
            try
            {
                metadatos = MetadatosService.ObtenerPorIdConsumidorAsync(idConsumidor).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (metadatos == null)
            {
                return NotFound();
            }

            return Ok(metadatos);
        }

        [HttpGet]
        [ActionName("porcanciongustada")]
        public IActionResult GetMetadataCancionGustadaPorIdConsumidor(string idConsumidor)
        {
            if (idConsumidor == null)
            {
                return BadRequest("idConsumidor no puede ser null");
            }

            List<Metadatos> metadatos;
            try
            {
                metadatos = MetadatosService.ObtenerMeGustaPorIdConsumidorAsync(idConsumidor).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine("MetadatosController: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (metadatos == null)
            {
                return NotFound();
            }

            return Ok(metadatos);
        }

        [HttpPost]
        [ActionName("")]
        public IActionResult Crear(Metadatos metadatos)
        {
            if (metadatos == null)
            {
                return BadRequest("body no puede ser null");
            }

            Metadatos resultado;
            try
            {
                resultado = MetadatosService.Crear(metadatos).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        [HttpPut]
        [ActionName("")]
        public IActionResult Actualizar(string id, Metadatos metadatos)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            bool resultado;
            try
            {
                resultado = MetadatosService.ActualizarAsync(id, metadatos).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
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
                resultado = MetadatosService.BorrarAsync(id).Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en MetadatosService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
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
