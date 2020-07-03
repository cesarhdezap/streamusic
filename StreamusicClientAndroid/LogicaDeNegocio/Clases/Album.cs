using Logica.ServiciosDeComunicacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Clases
{
    public class Album
    {
        public string Id { get; set; }

        public string IdArtista { get; set; }

        public byte[] Ilustracion { get; set; }

        public string Nombre { get; set; }

        public int AñoDeLanzamiento { get; set; }

        public string CompañiaDiscografica { get; set; }

        public override string ToString()
        {
            return Nombre;
        }

        public List<Cancion> Canciones { get; set; }

        public Artista Artista { get; set; }

        public Album()
        {
            Canciones = new List<Cancion>();
        }

        public void CargarArtista()
        {
            APIGatewayService api = new APIGatewayService();
            Artista = api.ObtenerArtistaPorId(IdArtista);
        }

        public void CargarCancionesConAlbumYArtistas()
        {
            APIGatewayService api = new APIGatewayService();
            Canciones = api.ObtenerCancionesPorIdAlbum(Id);
            Canciones.ForEach(c => c.Album = this);
            Canciones.ForEach(c => {
                try
                {
                    c.Artistas = api.ObtenerArtistasPorIdCancion(c.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
