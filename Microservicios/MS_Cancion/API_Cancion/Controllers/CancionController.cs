using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using API_Cancion.Services;
using API_Cancion.Models;

namespace API_Cancion.Controllers
{
    [Route("api/canciones")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly CancionService _cancionService;

        public UsuariosController(CancionService cancionService)
        {
            _cancionService = cancionService;
        }

        [HttpGet]
        public ActionResult<List<Cancion>> Get() => _cancionService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCancion")]
        public ActionResult<Cancion> Get(string id)
        {
            var cancion = _cancionService.Get(id);

            if (cancion == null)
            {
                return NotFound();
            }

            return cancion;
        }

        [HttpPost]
        public ActionResult<Cancion> Create([FromBody] Cancion cancion)
        {
            _cancionService.Create(cancion);

            return CreatedAtRoute("GetCancion", new { id = cancion.Id.ToString() }, cancion);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Cancion cancionActualizar)
        {
            var usuario = _cancionService.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _cancionService.Update(id, cancionActualizar);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var cancion = _cancionService.Get(id);

            if (cancion == null)
            {
                return NotFound();
            }

            _cancionService.Remove(cancion.Id);

            return NoContent();
        }
    }
}
