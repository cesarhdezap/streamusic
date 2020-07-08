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
using Org.Apache.Http.Conn;
using StreamusicClientAndroid.Interfacez;
using StreamusicClientAndroid.RecyclerViewAdapters;

namespace StreamusicClientAndroid
{
    public class VerMisListasFragment : Android.Support.V4.App.Fragment
    {
        
        Usuario Usuario;
        IReproductor Reproductor;
        ICambiarContenido CambiarContenido;
        public VerMisListasFragment(Usuario usuario, IReproductor reproductor, ICambiarContenido cambiarContenido)
        {
            Reproductor = reproductor;
            CambiarContenido = cambiarContenido;
            Usuario = usuario;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_vermislistas, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            APIGatewayService api = new APIGatewayService();
            var listas = api.ObtenerTodasLasListasPorIdUsuario(Usuario.Id); //Excepcion
            
            if(listas != null)
            {
                listas.RemoveAll(l => l.EsHistorialDeReproduccion);
            }
            else
            {
                listas = new List<ListaDeReproduccion>();
            }

            var buttonMiHistorial = View.FindViewById<Button>(Resource.Id.buttonMiHistorial);
            buttonMiHistorial.Click += ButtonMiHistorial_Click;


            var recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerViewListasDeReproduccion);
            ListasDeReproduccionRecyclerViewAdapter adapter = new ListasDeReproduccionRecyclerViewAdapter(listas.ToArray());
            adapter.ItemClick += ListaDeReproduccionAdapter_ItemClick;
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(View.Context, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(adapter);
        }

        private void ListaDeReproduccionAdapter_ItemClick(object sender, ListasDeReproduccionRecyclerViewAdapterClickEventArgs e)
        {
            e.ListaDeReproduccion.CargarCancionesConArtistasYAlbum();

            var listasFragment = new ListasFragment(e.ListaDeReproduccion.Canciones, e.ListaDeReproduccion.Nombre, null, Reproductor, Usuario, CambiarContenido);
            CambiarContenido.CambiarContenido(listasFragment);
        }

        private void ButtonMiHistorial_Click(object sender, EventArgs e)
        {
            APIGatewayService api = new APIGatewayService();
            var listas = api.ObtenerTodasLasListasPorIdUsuario(Usuario.Id); //Excepcion
            if (listas == null)
            {
                listas = new List<ListaDeReproduccion>();
            }

            var historial = listas.FirstOrDefault(l => l.EsHistorialDeReproduccion);
            if (historial != null)
            {
                historial.CargarCancionesConArtistasYAlbum();

                var listasFragment = new ListasFragment(historial.Canciones, "Historial de reproducción", null, Reproductor, Usuario, CambiarContenido);
                CambiarContenido.CambiarContenido(listasFragment);
            }
            else
            {
                Toast.MakeText(View.Context, "No existe un historial de reproducción.", ToastLength.Short).Show();
            }
        }
    }
}