using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS_HistorialDeReproduccion.Models;
using MS_HistorialDeReproduccion.Services;

namespace MS_HistorialDeReproduccion.Controllers
{
    [Route("api/listasdereproduccion")]
    [ApiController]
    public class ListaDeReproduccionController : ControllerBase
    {
        private readonly ListaDeReproduccionService _listaDeReproduccionService;

        public ListaDeReproduccionController(ListaDeReproduccionService listaDeReproduccionService)
        {
            _listaDeReproduccionService = listaDeReproduccionService;
        }

        [HttpGet]
        public ActionResult<List<ListaDeReproduccion>> Get() => _listaDeReproduccionService.Get();

        [HttpGet("{id:length(24)}", Name = "GetListaDeReproduccion")]
        public ActionResult<ListaDeReproduccion> Get(string id)
        {
            var lista = _listaDeReproduccionService.Get(id);

            if (lista == null)
            {
                return NotFound();
            }

            return lista;
        }

        [HttpPost]
        public ActionResult<ListaDeReproduccion> Create([FromBody] ListaDeReproduccion lista)
        {
            _listaDeReproduccionService.Create(lista);

            return CreatedAtRoute("GetListaDeReproduccion", new { id = lista.Id.ToString() }, lista);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ListaDeReproduccion lista)
        {
            var usuario = _listaDeReproduccionService.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _listaDeReproduccionService.Update(id, lista);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var lista = _listaDeReproduccionService.Get(id);

            if (lista == null)
            {
                return NotFound();
            }

            _listaDeReproduccionService.Remove(lista.Id);

            return NoContent();
        }

    }
}
