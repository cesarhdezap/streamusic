using Logica.ServiciosDeComunicacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Clases
{
    public class ListaDeReproduccion
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> IdsCanciones { get; set; }
        public string IdUsuario { get; set; }
        public bool EsHistorialDeReproduccion { get; set; }

        public List<Cancion> Canciones { get; set; }

        public ListaDeReproduccion()
        {
            Canciones = new List<Cancion>();
            IdsCanciones = new List<string>();
        }

        public bool CargarCancionesConArtistasYAlbum()
        {
            APIGatewayService api = new APIGatewayService();
            bool huboExcepcion = false;

            if(Canciones != null)
            {
                Canciones.Clear();
            }
            else
            {
                Canciones = new List<Cancion>();
            }

            foreach (string idCancion in IdsCanciones)
            {
                Cancion cancion = null;
                try
                {
                    cancion = api.ObtenerCancionPorId(idCancion);
                }
                catch (Exception)
                {
                    huboExcepcion = true;
                    break;
                }

                try
                {
                    cancion.Artistas = api.ObtenerArtistasPorIdCancion(idCancion);
                }
                catch (Exception)
                {
                    huboExcepcion = true;
                }

                try
                {
                    cancion.Album = api.ObtenerAlbumPorIdCancion(idCancion);
                }
                catch (Exception)
                {
                    huboExcepcion = true;
                }

                Canciones.Add(cancion);
            }

            return huboExcepcion;
        }

        public override string ToString()
        {
            return Nombre;
        }

    }
}
