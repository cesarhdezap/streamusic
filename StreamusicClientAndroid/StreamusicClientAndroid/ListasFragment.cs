using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Logica.Clases;
using StreamusicClientAndroid.Interfacez;

namespace StreamusicClientAndroid
{
    public class ListasFragment : Android.Support.V4.App.Fragment
    {
        List<Cancion> Canciones;
        string Titulo;
        byte[] DatosImagen;
        IReproductor Reproductor;

        public ListasFragment(List<Cancion> canciones, string titulo, byte[] imagen, IReproductor reproductor)
        {
            Reproductor = reproductor;
            Canciones = canciones;
            Titulo = titulo;
            DatosImagen = imagen;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var txtTitulo = View.FindViewById<TextView>(Resource.Id.txtTituloLista);
            var imagen = View.FindViewById<ImageView>(Resource.Id.imageViewImagenLista);
            txtTitulo.Text = Titulo;
            imagen.SetImageBitmap(BitmapFactory.DecodeByteArray(DatosImagen, 0, DatosImagen.Length));
            var buttonReproducir = View.FindViewById<Button>(Resource.Id.buttonReproducirListaDeCanciones);
            buttonReproducir.Click += ButtonReproducir_Click;

            var recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerViewListaCanciones);
            ListaDeCancionesRecyclerViewAdapter adapter = new ListaDeCancionesRecyclerViewAdapter(Canciones.ToArray());
            //adapter.ItemLongClick += ListaDeCancionesAdapter_ItemLongClick;
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(View.Context, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(adapter);
            
            RegisterForContextMenu(recyclerView);
        }

        private void ButtonReproducir_Click(object sender, EventArgs e)
        {
            //MandarAReproducir la lista de canciones
            throw new NotImplementedException();
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            
            bool resultado = base.OnContextItemSelected(item);
            if (item.ItemId == (int)IdsContextMenu.AgregarAlFinalDeCola)
            {
                Toast.MakeText(View.Context, "Finalcola", ToastLength.Short);
            }
            else if(item.ItemId == (int)IdsContextMenu.AgregarSiguienteEnCola)
            {
                Toast.MakeText(View.Context, "siguiente en cola", ToastLength.Short);
            }
            else if (item.ItemId == (int)IdsContextMenu.AgregarALista)
            {
                Toast.MakeText(View.Context, "agregar a lista", ToastLength.Short);
            }
            return resultado;
        }

        private void ListaDeCancionesAdapter_ItemLongClick(object sender, ListaDeCancionesRecyclerViewAdapterClickEventArgs e)
        {
            Toast.MakeText(View.Context, e.Cancion.Nombre, ToastLength.Short).Show();
            
            //Mostrar menu de agregar a siguiente, cola, agregar a lista.
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_listas, container, false);
        }
    }
}