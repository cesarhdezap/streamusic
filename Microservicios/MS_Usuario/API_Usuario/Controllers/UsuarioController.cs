using Microsoft.AspNetCore.Mvc;
using MS_Usuario.Models;
using MS_Usuario.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace MS_Usuario.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get() => _usuarioService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUsuario")]
        public ActionResult<Usuario> Get(string id)
        {
            var usuario = _usuarioService.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPost]
        public ActionResult<Usuario> Create([FromBody] Usuario usuario)
        {
            _usuarioService.Create(usuario);

            return CreatedAtRoute("GetUsuario", new { id = usuario.Id.ToString() }, usuario);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Usuario usuarioActualizar)
        {
            var usuario = _usuarioService.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _usuarioService.Update(id, usuarioActualizar);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var usuario = _usuarioService.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _usuarioService.Remove(usuario.Id);

            return NoContent();
        }
    }
}
