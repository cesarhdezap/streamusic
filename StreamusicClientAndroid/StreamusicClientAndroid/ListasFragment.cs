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
using Java.Lang;
using Logica;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using StreamusicClientAndroid.Interfacez;

namespace StreamusicClientAndroid
{
    public class ListasFragment : Android.Support.V4.App.Fragment
    {
        List<Cancion> Canciones;
        string Titulo;
        byte[] DatosImagen;
        IReproductor Reproductor;
        Usuario Usuario;

        public ListasFragment(List<Cancion> canciones, string titulo, byte[] imagen, IReproductor reproductor, Usuario usuario)
        {
            Usuario = usuario;
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
            adapter.ItemLongClick += ListaDeCancionesAdapter_ItemLongClick;
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(View.Context, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(adapter);
        }

        private void ButtonReproducir_Click(object sender, EventArgs e)
        {
            //MandarAReproducir la lista de canciones
            throw new NotImplementedException();
        }

        private void ListaDeCancionesAdapter_ItemLongClick(object sender, ListaDeCancionesRecyclerViewAdapterClickEventArgs e)
        {
            Android.App.AlertDialog.Builder alerta = new Android.App.AlertDialog.Builder(View.Context);
            alerta.SetTitle("Seleccione una opción");

            string[] items = { "Agregar al final de cola", "Agregar a siguiente en cola", "Agregar a lista" };

            alerta.SetItems(items, new EventHandler<DialogClickEventArgs> (delegate (object o, DialogClickEventArgs args) 
            {
                if(args.Which == (int)OpcionesLista.AgregarAlFinalDeCola)
                {
                    Reproductor.AñadirAlFinal(e.Cancion);
                }
                else if (args.Which == (int)OpcionesLista.AgregarASiguienteEnCola)
                {
                    Reproductor.AñadirSiguiente(e.Cancion);
                }
                else if(args.Which == (int)OpcionesLista.AgregarALista)
                {
                    MostrarSeleccionDeListasDeReproduccionYAgregarla(e.Cancion);
                }
                else
                {
                    Toast.MakeText(View.Context, "Error al manejar alerta.", ToastLength.Long);
                }
                
            }));
            alerta.Show();
        }

        private void MostrarSeleccionDeListasDeReproduccionYAgregarla(Cancion cancion)
        {
            APIGatewayService api = new APIGatewayService();
            bool huboExcepcionCargandoListas = false;
            List<ListaDeReproduccion> listas = null;
            try
            {
                listas = api.ObtenerTodasLasListasPorIdUsuario(Usuario.Id);
            }
            catch (System.Exception ex)
            {
                huboExcepcionCargandoListas = true;
            }

            if (!huboExcepcionCargandoListas && listas != null)
            {
                listas.RemoveAll(l => l.EsHistorialDeReproduccion);

                List<string> nombresDeListas = new List<string>();
                foreach (var lista in listas)
                {
                    nombresDeListas.Add(lista.Nombre);
                }

                Android.App.AlertDialog.Builder alerta = new Android.App.AlertDialog.Builder(View.Context);
                alerta.SetTitle("Seleccione la lista");

                alerta.SetItems(nombresDeListas.ToArray(), new EventHandler<DialogClickEventArgs>(delegate (object o, DialogClickEventArgs args)
                {
                    var lista = listas[args.Which];
                    lista.IdsCanciones.Add(cancion.Id);
                    bool resultado = false;
                    bool huboExcepcion = false;

                    try
                    {
                        resultado = api.ActualizarListaDeReproduccion(lista.Id, lista);
                    }
                    catch (System.Exception ex)
                    {
                        huboExcepcion = true;
                    }

                    if (!huboExcepcion)
                    {
                        if (resultado)
                        {
                            Toast.MakeText(View.Context, "Cancion añadida correctamente.", ToastLength.Short);
                        }
                        else
                        {
                            Toast.MakeText(View.Context, "Lo sentimos, no se pudo agregar la cancion a la lista.", ToastLength.Short);
                        }
                    }
                    else
                    {
                        Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Short);
                    }

                }));
                alerta.Show();
            }
            else if(listas == null)
            {
                Toast.MakeText(View.Context, "No hay ninguna lista de canciones.", ToastLength.Short);
            }
            else
            {
                Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Short);
            }
        }

        public enum OpcionesLista
        {
            AgregarAlFinalDeCola,
            AgregarASiguienteEnCola,
            AgregarALista
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_listas, container, false);
        }
    }
}