using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS_Album.Models;
using MS_Album.Services;

namespace MS_Album.Controllers
{
    [Route("api/albumes")]
    [ApiController]
    public class AlbumesController : ControllerBase
    {
        private readonly AlbumService _albumService;

        public AlbumesController(AlbumService bookService)
        {
            _albumService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Album>> Get() =>
            _albumService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Album> Get(string id)
        {
            var book = _albumService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Album> Create(Album book)
        {
            _albumService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Album bookIn)
        {
            var book = _albumService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _albumService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _albumService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _albumService.Remove(book.Id);

            return NoContent();
        }
    }
}
