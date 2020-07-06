using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS_Artista.Models;
using MS_Artista.Services;

namespace MS_Artista.Controllers
{
    [Route("api/artistas")]
    [ApiController]
    public class ArtistasController : ControllerBase
    {
        private readonly ArtistaService _artistaService;

        public ArtistasController(ArtistaService artistaService)
        {
            _artistaService = artistaService;
        }

        [HttpGet]
        public ActionResult<List<Artista>> Get() => _artistaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetArtista")]
        public ActionResult<Artista> Get(string id)
        {
            var artista = _artistaService.Get(id);

            if (artista == null)
            {
                return NotFound();
            }

            return artista;
        }

        [HttpPost]
        public ActionResult<Artista> Create(Artista artista)
        {
            _artistaService.Create(artista);

            return CreatedAtRoute("GetArtista", new { id = artista.Id.ToString() }, artista);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Artista artistaIn)
        {
            
            var artista = _artistaService.Get(id);

            if (artista == null)
            {
                return NotFound();
            }

            artistaIn.Id = id;

            _artistaService.Update(id, artistaIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var artista = _artistaService.Get(id);

            if (artista == null)
            {
                return NotFound();
            }

            _artistaService.Remove(artista.Id);

            return NoContent();
        }
    }
}
