using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MS_Busqueda.Services;


namespace MS_Busqueda.Controllers
{
    [Route("api/canciones/[action]")]
    [ApiController]
    public class CancionController : ControllerBase
    {
        private CancionService CancionService;

        public CancionController()
        {
            CancionService = new CancionService();
        }

        [HttpGet]
        [ActionName("")]
        public IActionResult GetAllCanciones()
        {
            var canciones = CancionService.GetAllCancionesAsync().Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion")]
        public IActionResult GetCancionesPorNombre(string nombreCancion)
        {
            var canciones = CancionService.GetCancionesPorNombre(nombreCancion).Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("artista")]
        public IActionResult GetCancionesPorArtista(string nombreArtista)
        {
            var canciones = CancionService.GetCancionesPorArtista(nombreArtista).Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }
        [HttpGet]
        [ActionName("album")]
        public IActionResult GetCancionesPorAlbum(string nombreAlbum)
        {
            var canciones = CancionService.GetCancionesPorAlbum(nombreAlbum).Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("idAlbum")]
        public IActionResult GetCancionesPorIdAlbum(string idAlbum)
        {
            var canciones = CancionService.GetCancionesPorIdAlbum(idAlbum).Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("idArtista")]
        public IActionResult GetCancionesPorIdArtista(string idArtista)
        {
            var canciones = CancionService.GetCancionesPorIdArtista(idArtista).Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        } 

        [HttpGet]
        [ActionName("id")]
        public IActionResult GetCancionPorId(string id)
        {
            var canciones = CancionService.GetCancionPorId(id).Result;
            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

    }
}
