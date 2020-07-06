using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("buscar/[action]")]
    [ApiController]
    public class BusquedasController : ControllerBase
    {
        BusquedasService Service;

        public BusquedasController()
        {
            Service = new BusquedasService();
        }

        [HttpGet]
        [ActionName("cancion")]
        public IActionResult GetAllCanciones()
        {            
            List<Cancion> canciones;
            try
            {
                canciones = Service.GetAllCanciones();
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion/pornombre")]
        public IActionResult GetCancionesPorNombre(string nombreCancion)
        {
            if (nombreCancion == null)
            {
                return BadRequest("nombreCancion no puede ser null");
            }

            List<Cancion> canciones;
            try
            {
                canciones = Service.GetCancionesPorNombre(nombreCancion);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion/porartista")]
        public IActionResult GetCancionesPorArtista(string nombreArtista)
        {
            if (nombreArtista == null)
            {
                return BadRequest("nombreArtista no puede ser null");
            }

            List<Cancion> canciones;
            try
            {
                canciones = Service.GetCancionesPorArtista(nombreArtista);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion/poridalbum")]
        public IActionResult GetCancionesPorIdAlbum(string idAlbum)
        {
            if (idAlbum == null)
            {
                return BadRequest("idAlbum no puede ser null");
            }

            List<Cancion> canciones;
            try
            {
                canciones = Service.GetCancionesPorIdAlbum(idAlbum);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion/poridartista")]
        public IActionResult GetCancionesPorIdArtista(string idArtista)
        {
            if (idArtista == null)
            {
                return BadRequest("idArtista no puede ser null");
            }

            List<Cancion> canciones;
            try
            {
                canciones = Service.GetCancionesPorIdArtista(idArtista);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion/poralbum")]
        public IActionResult GetCancionesPorAlbum(string nombreAlbum)
        {
            if (nombreAlbum == null)
            {
                return BadRequest("nombreAlbum no puede ser null");
            }

            List<Cancion> canciones;
            try
            {
                canciones = Service.GetCancionesPorAlbum(nombreAlbum);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (canciones == null)
            {
                return NotFound();
            }

            return Ok(canciones);
        }

        [HttpGet]
        [ActionName("cancion/porid")]
        public IActionResult GetCancionPorId(string id)
        {
            if (id == null)
            {
                return BadRequest("Id no puede ser null");
            }

            Cancion cancion;
            try
            {
                cancion = Service.GetCancionPorId(id);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (cancion == null)
            {
                return NotFound();
            }

            return Ok(cancion);
        }


        [HttpGet]
        [ActionName("artista")]
        public IActionResult GetAllArtistas()
        {
            List<Artista> artistas;
            try
            {
                artistas = Service.GetAllArtistas().Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (artistas == null)
            {
                return NotFound();
            }

            return Ok(artistas);
        }

        [HttpGet]
        [ActionName("artista/pornombre")]
        public IActionResult GetArtistasPorNombre(string nombreArtista)
        {
            if (nombreArtista == null)
            {
                return BadRequest("nombreArtista no puede ser null");
            }

            List<Artista> artistas;
            try
            {
                artistas = Service.GetArtistasPorNombre(nombreArtista);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (artistas == null)
            {
                return NotFound();
            }

            return Ok(artistas);
        }

        [HttpGet]
        [ActionName("artista/porid")]
        public IActionResult GetArtistaPorId(string id)
        {
            if (id == null)
            {
                return BadRequest("Id no puede ser null");
            }

            Artista artista;
            try
            {
                artista = Service.GetArtistaPorId(id);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (artista == null)
            {
                return NotFound();
            }

            return Ok(artista);
        }

        [HttpGet]
        [ActionName("artista/poridcancion")]
        public IActionResult GetArtistasPorIdCancion(string idCancion)
        {
            if (idCancion == null)
            {
                return BadRequest("idCancion no puede ser null");
            }

            List<Artista> artistas;
            try
            {
                artistas = Service.GetArtistasPorIdCancion(idCancion);
            }
            catch(Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (artistas == null)
            {
                return NotFound();
            }

            return Ok(artistas);
        }


        [HttpGet]
        [ActionName("album")]
        public IActionResult GetAllAlbums()
        {
            List<Album> albums;
            try
            {
                albums = Service.GetAllAlbums().Result;
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("album/pornombre")]
        public IActionResult GetAlbumsPorNombre(string nombreAlbum)
        {
            if (nombreAlbum == null)
            {
                return BadRequest("nombreAlbum no puede ser null");
            }

            List<Album> albums;
            try
            {
                albums = Service.GetAlbumsPorNombre(nombreAlbum);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("album/pornombreartista")]
        public IActionResult GetAlbumsPorArtista(string nombreArtista)
        {
            if (nombreArtista == null)
            {
                return BadRequest("nombreArtista no puede ser null");
            }

            List<Album> albums;
            try
            {
                albums = Service.GetAlbumsPorArtista(nombreArtista);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("album/poridartista")]
        public IActionResult GetAlbumsPorIdArtista(string idArtista)
        {
            if (idArtista == null)
            {
                return BadRequest("idArtista no puede ser null");
            }

            List<Album> albums;
            try
            {
                albums = Service.GetAlbumsPorIdArtista(idArtista);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        [HttpGet]
        [ActionName("album/porid")]
        public IActionResult GetAlbumPorId(string id)
        {
            if (id == null)
            {
                return BadRequest("id no puede ser null");
            }

            Album artista;
            try
            {
                artista = Service.GetAlbumPorId(id);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (artista == null)
            {
                return NotFound();
            }

            return Ok(artista);
        }

        [HttpGet]
        [ActionName("album/poridcancion")]
        public IActionResult GetAlbumPorIdCancion(string idCancion)
        {
            if (idCancion == null)
            {
                return BadRequest("idCancion no puede ser null");
            }

            Album artista;
            try
            {
                artista = Service.GetAlbumPorIdCancion(idCancion);
            }
            catch (Exception e)
            {
                return Conflict("Error en BusquedasService, Mensaje: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace);
            }

            if (artista == null)
            {
                return NotFound();
            }

            return Ok(artista);
        }
    }
}
