using Logica.ServiciosDeComunicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Logica.Clases
{
    public class Artista
    {
        public string Id { get; set; }
        public byte[] Ilustracion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public List<Cancion> Canciones { get; set; }

        public Artista()
        {
            Canciones = new List<Cancion>();
        }

        public void CargarCancionesConAlbumYArtistas()
        {
            APIGatewayService api = new APIGatewayService();
            Canciones = api.ObtenerCancionesPorIdArtista(Id);
            Canciones.ForEach(c =>{
                try
                {
                    c.Artistas = api.ObtenerArtistasPorIdCancion(c.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            Canciones.ForEach(c => {
                try
                {
                    c.Album = api.ObtenerAlbumPorIdCancion(c.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
