using Logica.ServiciosDeComunicacion;
using Logica.Utilerias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Logica.Utilerias.UtileriasDeArchivos;


namespace Logica.Clases
{
    public class Cancion
    {
        public string Id { get; set; }
        public string IdAlbum { get; set; }
        public List<string> IdsArtistas { get; set; }
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public int Calificacion { get; set; }
        public int VecesEscuchada { get; set; }
        public double Duracion { get; set; }
        public string IdArchivo { get; set; }
        public bool EsPublico { get; set; }

        public List<Artista> Artistas { get; set; }
        public Album Album { get; set; }
        public Metadatos Metadatos { get; set; }
        

        public Cancion()
        {
            Artistas = new List<Artista>();
            IdsArtistas = new List<string>();
        }

        public void CargarMetadatosDeLaCancion(string idUsuario)
        {
            APIGatewayService api = new APIGatewayService();
            Metadatos = api.CargarMetadatosPorIdCancionYIdUsuario(Id, idUsuario);
        }

        public void DescargarArchivoDeCancion()
        {
            byte[] archivo = null;
            bool errorDeConectividad = false;
            APIGatewayService api = new APIGatewayService();
            try
            {
                archivo = api.DescargarArchivoPorId(IdArchivo);
            }
            catch (Exception)
            {
                errorDeConectividad = true;
            }
            if (!errorDeConectividad)
            {
                UtileriasDeArchivos.GuardarArchivo(IdArchivo, archivo);
            }
        }

        public void EliminarArchivoDeCancion()
        {
            try
            {
                UtileriasDeArchivos.BorrarArchivo(IdArchivo);
            }
            catch(Exception e)
            {
                Console.WriteLine("No se encontro el archivo de cancion a eliminar. " + e.Message);
            }
        }
    }
}
