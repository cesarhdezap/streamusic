using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS_Archivo;
using MS_DescargaDeCanciones.Services;

namespace MS_DescargaDeCanciones.Controllers
{
    [Route("api/descargadecanciones")]
    [ApiController]
    public class DescargaDeCancionesController : ControllerBase
    {
        private DescargaDeCancionesService CancionesService;
        public DescargaDeCancionesController()
        {
            CancionesService = new DescargaDeCancionesService();
        }

        [HttpGet]
        public IActionResult Get(string id)
        {
            byte[] datos;

            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            try
            {
                datos = CancionesService.DescargarCancion(id).Result;
            }
            catch (AggregateException e)
            {
                Console.WriteLine("MS_DescargaDeCanciones: Error al contactar con archivo " + e.Message);
                return Conflict("MS_DescargaDeCanciones: Error al contactar con archivo " + e.Message);
            }

            if (datos == null)
            {
                return NotFound();
            }

            return Ok(datos);
        }
    }
}
