using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("descargarcancion")]
    [ApiController]
    public class DescargaDeCancionesController : ControllerBase
    {
        private DescargaDeCancionesService Service;

        public DescargaDeCancionesController()
        {
            Service = new DescargaDeCancionesService();
        }

        [HttpGet]
        public IActionResult Get(string id)
        {
            byte[] cancion;

            if (id == null)
            {
                return BadRequest("Id no puede ser null");
            }

            try
            {
                cancion = Service.DescargarCancion(id).Result;
            }
            catch (AggregateException e)
            {
                return Conflict("Error en DescargaDeCancionesService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }
            catch(Exception e)
            {
                return Conflict("Error en DescargaDeCancionesService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (cancion == null)
            {
                return NotFound();
            }

            return Ok(cancion);
        }
    }
}
