using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("autenticar")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private AutenticacionService AutenticacionService;
        public AutenticacionController()
        {
            AutenticacionService = new AutenticacionService();
        }

        [HttpGet]
        public IActionResult Autenticar(string nombreDeUsuario, string contraseña)
        {
            RespuestaLogin resultado;
            if(nombreDeUsuario == null || contraseña == null)
            {
                return BadRequest("Parametros nulos");
            }
            try
            {
                resultado = AutenticacionService.AutenticarUsuario(nombreDeUsuario, contraseña).Result;
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerExceptions);
                return Conflict("Error en AutenticarService, Mensaje: " + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

    }
}
