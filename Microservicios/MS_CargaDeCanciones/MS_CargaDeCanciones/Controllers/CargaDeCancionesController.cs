using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS_CargaDeCanciones.Services;

namespace MS_CargaDeCanciones.Controllers
{
    [Route("api/cargadecanciones")]
    [ApiController]
    public class CargaDeCancionesController : ControllerBase
    {
        private CargaDeCancionesService CargaDeCanciones;

        public CargaDeCancionesController()
        {
            CargaDeCanciones = new CargaDeCancionesService();
        }

        [HttpPost]
        public IActionResult Post(byte[] cancion)
        {

            if (cancion == null)
            {
                return BadRequest("cancion no puede ser null");
            }

            string id;
            try
            {
                id = CargaDeCanciones.CargarCancion(cancion).Result;
            }
            catch (AggregateException e)
            {
                return Conflict("Error al contactar con archivo " + e.Message);
            }

            if (id == null)
            {
                return NotFound();
            }

            return Ok(id);
        }
    }
}
