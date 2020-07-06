using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("cargarcancion")]
    [ApiController]
    public class CargaDeCancionesController : ControllerBase
    {
        private CargaDeCancionesService Service;

        public CargaDeCancionesController()
        {
            Service = new CargaDeCancionesService();
        }

        [HttpPost]
        public IActionResult Post(byte[] cancion)
        {
            string id;
            try
            {
                id = Service.CargarCancion(cancion).Result;
            }
            catch (AggregateException e)
            {
                return Conflict("Error en CargaDeCancionesService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace:" + e.StackTrace);
            }

            if (id == null)
            {
                BadRequest(id);
            }

            return Ok(id);
        }

    }
}
