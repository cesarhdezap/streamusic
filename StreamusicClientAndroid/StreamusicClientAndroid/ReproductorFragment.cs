using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Logica;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using Logica.Utilerias;
using StreamusicClientAndroid.Interfacez;

namespace StreamusicClientAndroid
{
    public class ReproductorFragment : Android.Support.V4.App.Fragment, IReproductor
    {
        public Usuario Usuario;

        private List<Cancion> Canciones;
        int IndiceActual;
        bool RepetirCancionActivado = false;
        bool AleatorizarActivado = false;

        Task ActualizadorDeSlider;
        Task VerificadorDeFinDeCancion;
        bool TokenDeCancelacion;
        bool CancionCorriendo;

        const int VALOR_MAXIMO_SLIDER_TIEMPO = 1000;
        const int VALOR_MAXIMO_SLIDER_VOLUMEN = 10;
        bool UsuarioMoviendoSliderBarraDeEstado = false;

        ListaDeReproduccion HistorialDeReproduccion;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var seekBar = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            seekBar.Max = VALOR_MAXIMO_SLIDER_TIEMPO;

            var txtCancion = View.FindViewById<TextView>(Resource.Id.txtNombreCancion);
            var txtArtista = View.FindViewById<TextView>(Resource.Id.txtNombreArtista);
            var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
            var buttonRepetir = View.FindViewById<ImageButton>(Resource.Id.ibtnRepetir);
            var buttonSiguiente = View.FindViewById<ImageButton>(Resource.Id.ibtnSiguiente);
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            var buttonAnterior = View.FindViewById<ImageButton>(Resource.Id.ibtnAnterior);
            var buttonBarajar = View.FindViewById<ImageButton>(Resource.Id.ibtnBarajar);

        }

        

        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.fragment_reproductor, container, false);
        }

        private void Reproducir(Cancion cancion)
        {
            byte[] archivo = null;
            bool huboExcepcion = false;
            if (!UtileriasDeArchivos.CancionYaDescargada(cancion.IdArchivo))
            {
                APIGatewayService api = new APIGatewayService();
                try
                {
                    archivo = api.DescargarArchivoPorId(cancion.IdArchivo);
                }
                catch (Exception)
                {
                    Toast.MakeText(View.Context, "Error al realizar la descarga, intente de nuevo mas tarde", ToastLength.Long).Show();
                    huboExcepcion = true;
                }
            }
            else
            {
                try
                {
                    archivo = UtileriasDeArchivos.LeerArchivoPorId(cancion.IdArchivo);
                }
                catch (ArgumentException)
                {
                    huboExcepcion = true;
                }
            }

            if (!huboExcepcion && archivo != null && archivo.Length > 0)
            {
                var txtCancion = View.FindViewById<TextView>(Resource.Id.txtNombreCancion);
                txtCancion.Text = cancion.Nombre;
                var txtArtista = View.FindViewById<TextView>(Resource.Id.txtNombreArtista);
                txtArtista.Text = cancion.Album.Nombre;

                try
                {
                    //EliminarDisposables();
                    //Mp3FileReader = new Mp3FileReader(new MemoryStream(archivo));
                    //Wave32 = new WaveChannel32(Mp3FileReader, (float)SliderVolumen.Value / VALOR_MAXIMO_SLIDER_VOLUMEN, 0);
                    //DirectSoundOut = new DirectSoundOut();
                    //DirectSoundOut.Init(Wave32);
                    //DirectSoundOut.Play();
                    CancionCorriendo = true;

                    //VerificadorDeFinDeCancion = new Task(() => VerificarFinDeCancion());
                    //VerificadorDeFinDeCancion.Start();
                    //ActualizadorDeSlider = new Task(() => ActualizarSliderBarraDeEstado());
                    //ActualizadorDeSlider.Start();

                    //TimeSpan tiempoActual = Wave32.TotalTime;
                    //LabelTiempoDeCancionTotal.Content = String.Format("{0}:{1:D2}", tiempoActual.Minutes, tiempoActual.Seconds);
                }
                catch (Exception e)
                {
                    Toast.MakeText(View.Context, e.Message, ToastLength.Long).Show();
                }

                //AgregarCancionAHistorial(cancion.Id);
            }
        }

        public void ReproducirLista(List<Cancion> lista, int indice = 0)
        {
            Canciones = lista;
            IndiceActual = indice;
            Reproducir(Canciones[IndiceActual]);
        }

        public void AñadirAlFinal(Cancion cancion)
        {
            throw new NotImplementedException();
        }

        public void AñadirSiguiente(Cancion cancion)
        {
            throw new NotImplementedException();
        }
    }
}