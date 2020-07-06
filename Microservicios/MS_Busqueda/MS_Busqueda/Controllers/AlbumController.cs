using Microsoft.AspNetCore.Mvc;
using MS_Busqueda.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Busqueda.Controllers
{
    [Route("api/albums/[action]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private AlbumService AlbumService;

        public AlbumController()
        {
            AlbumService = new AlbumService();
        }

        [HttpGet]
        [ActionName("")]
        public IActionResult GetAllAlbums()
        {
            var albums = AlbumService.GetAllAlbumsAsync().Result;
            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("album")]
        public IActionResult GetAlbumsPorNombre(string nombreAlbum)
        {
            var albums = AlbumService.GetAlbumsPorNombre(nombreAlbum).Result;
            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("idcancion")]
        public IActionResult GetAlbumPorIdCancion(string idCancion)
        {
            if (idCancion == null)
            {
                return BadRequest("La idCancion no puede ser null");
            }
            var album = AlbumService.GetAlbumPorIdCancion(idCancion).Result;

            if (album == null)
            {
                return NotFound("Cancion no encontrada");
            }

            return Ok(album);
        }

        [HttpGet]
        [ActionName("artista")]
        public IActionResult GetAlbumsPorArtista(string nombreArtista)
        {
            var albums = AlbumService.GetAlbumsPorArtista(nombreArtista).Result;
            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("id")]
        public IActionResult GetAlbumPorId(string id)
        {
            var albums = AlbumService.GetAlbumPorId(id).Result;
            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("idArtista")]
        public IActionResult GetAlbumPorIdArtista(string idArtista)
        {
            var albums = AlbumService.GetAlbumsPorIdArtista(idArtista).Result;
            if (albums == null)
            {
                return NotFound();
            }
            if(albums.Count <= 0)
            {
                return NotFound("La cantidad de albumes es 0");
            }

            return Ok(albums);
        }
    }
}
