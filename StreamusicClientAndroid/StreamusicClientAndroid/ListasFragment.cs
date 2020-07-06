using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Logica.Clases;

namespace StreamusicClientAndroid
{
    public class ListasFragment : Android.Support.V4.App.Fragment
    {
        List<Cancion> Canciones;
        string Titulo;
        byte[] DatosImagen;

        public ListasFragment(List<Cancion> canciones, string titulo, byte[] imagen)
        {
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
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_listas, container, false);
        }
    }
}