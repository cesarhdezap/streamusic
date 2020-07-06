using Microsoft.AspNetCore.Mvc;
using MS_Busqueda.Models;
using MS_Busqueda.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Busqueda.Controllers
{
    [Route("api/artistas/[action]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        private ArtistaService ArtistaService;

        public ArtistaController()
        {
            ArtistaService = new ArtistaService();
        }

        [HttpGet]
        [ActionName("")]
        public IActionResult GetAllArtistas()
        {
            var artistas = ArtistaService.GetAllArtistasAsync().Result;
            if (artistas == null)
            {
                return NotFound();
            }

            return Ok(artistas);
        }

        [HttpGet]
        [ActionName("idCancion")]
        public IActionResult GetArtistasPorIdCancion(string idCancion)
        {
            if (idCancion == null)
            {
                return BadRequest("La idCancion no puede ser null");
            }
            var artistas = ArtistaService.GetArtistasPorIdCancion(idCancion).Result;

            if (artistas == null)
            {
                return NotFound("Cancion no encontrada");
            }
            else if(artistas.Count <= 0)
            {
                return NotFound("La cancion no tiene artistas");
            }

            return Ok(artistas);
        }

        [HttpGet]
        [ActionName("artista")]
        public IActionResult GetArtistaPorNombre(string nombreArtista)
        {
            var artistas = ArtistaService.GetArtistaPorNombre(nombreArtista).Result;
            if (artistas == null)
            {
                return NotFound();
            }

            return Ok(artistas);
        }


        [HttpGet]
        [ActionName("id")]
        public IActionResult GetArtistaPorId(string id)
        {
            var artistas = ArtistaService.GetArtistaPorId(id).Result;
            if (artistas == null)
            {
                return NotFound();
            }

            return Ok(artistas);
        }
    }
}
