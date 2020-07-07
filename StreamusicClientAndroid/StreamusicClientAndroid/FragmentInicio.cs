using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Logica;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using StreamusicClientAndroid.Interfacez;

namespace StreamusicClientAndroid
{
    public class FragmentInicio : Android.Support.V4.App.Fragment
    {
        Usuario Usuario;
        IReproductor Reproductor;
        ICambiarContenido CambiarContenido;
        public FragmentInicio(Usuario usuario, IReproductor ireproductor, ICambiarContenido cambiarContenido)
        {
            Usuario = usuario;
            CambiarContenido = cambiarContenido;
            Reproductor = ireproductor;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            APIGatewayService api = new APIGatewayService();
            var albumes = api.ObtenerTodosLosAlbumes();

            RecyclerView recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerViewAlbumes);
            AlbumesRecyclerViewAdapter adapter = new AlbumesRecyclerViewAdapter(albumes.ToArray());
            adapter.ItemClick += AlbumAdapter_ItemClick;
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(View.Context, LinearLayoutManager.Horizontal, false);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(adapter);


            var artistas = api.ObtenerArtistas();

            RecyclerView recyclerViewArtista = View.FindViewById<RecyclerView>(Resource.Id.recyclerViewArtistas);
            ArtistasRecyclerViewAdapter adapterArtista = new ArtistasRecyclerViewAdapter(artistas.ToArray());
            adapterArtista.ItemClick += ArtistaAdapter_ItemClick;
            RecyclerView.LayoutManager layoutManagerArtista = new LinearLayoutManager(View.Context, LinearLayoutManager.Horizontal, false);
            recyclerViewArtista.SetLayoutManager(layoutManagerArtista);
            recyclerViewArtista.SetAdapter(adapterArtista);

        }

        private void ArtistaAdapter_ItemClick(object sender, ArtistasRecyclerViewAdapterClickEventArgs e)
        {
            APIGatewayService api = new APIGatewayService();
            List<Cancion> canciones = new List<Cancion>();
            canciones = api.ObtenerCancionesPorIdArtista(e.Artista.Id); // Excepcion

            canciones.ForEach(c => 
            {
                c.Artistas = new List<Artista>();
                c.Artistas.Add(e.Artista);
            });

            var listasFragment = new ListasFragment(canciones, e.Artista.Nombre, e.Artista.Ilustracion, Reproductor, Usuario, CambiarContenido);

            CambiarContenido.CambiarContenido(listasFragment);
        }

        private void AlbumAdapter_ItemClick(object sender, RecyclerViewAdapterClickEventArgs e)
        {
            
            APIGatewayService api = new APIGatewayService();
            List<Cancion> canciones = new List<Cancion>();
            canciones = api.ObtenerCancionesPorIdAlbum(e.Album.Id); // Excepcion

            canciones.ForEach(c =>
            {
                c.Artistas = api.ObtenerArtistasPorIdCancion(c.Id); // Excepcion
                c.Album = e.Album;
            });

            var listasFragment = new ListasFragment(canciones, e.Album.Nombre, e.Album.Ilustracion, Reproductor, Usuario, CambiarContenido);

            CambiarContenido.CambiarContenido(listasFragment);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_inicio, container, false);
        }
    }
}