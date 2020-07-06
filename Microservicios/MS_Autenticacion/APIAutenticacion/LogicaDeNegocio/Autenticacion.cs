using APIAutenticacion.AcesoADatos;
using APIAutenticacion.LogicaDeNegocio.Clases;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace APIAutenticacion.LogicaDeNegocio
{
    public class Autenticacion
    {
        public Usuario AutenticarUsuario(string nombre, string password)
        {
            ConexionMSUsuario conexion = new ConexionMSUsuario();

            conexion.Conectar();

            List<Usuario> usuarios = conexion.GetUsuariosAsync().Result;

            Usuario usuario = null;
            if (usuarios != null)
            {
                usuario = usuarios.FirstOrDefault(u => u.NombreDeUsuario == nombre);
            }


            Usuario usuarioARetornar = null;
            if (usuario != null)
            {
                if (usuario.Contraseña == password)
                {
                    usuarioARetornar = usuario;
                }
            }

            return usuarioARetornar;
        }
    }
}