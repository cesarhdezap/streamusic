using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Nio;
using Java.Util;
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
        ICambiarContenido CambiarContenido;
        private List<Cancion> Canciones;
        int IndiceActual;
        bool RepetirCancionActivado = false;
        bool AleatorizarActivado = false;
        MediaPlayer Reproductor = new MediaPlayer();
        System.Timers.Timer ActualizadorDeSlider = new System.Timers.Timer();
        bool TokenDeCancelacion;
        bool CancionCorriendo = false ;
        ListasFragment ListasFragment;


        const int VALOR_MAXIMO_SLIDER_TIEMPO = 1000;
        ListaDeReproduccion HistorialDeReproduccion;

        public ReproductorFragment(Usuario usuario, ICambiarContenido cambiarContenido)
        {
            Usuario = usuario;
            CambiarContenido = cambiarContenido;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActualizadorDeSlider.Interval = 500;
            ActualizadorDeSlider.Elapsed += ActualizadorDeSlider_Elapsed;
            ActualizadorDeSlider.Enabled = false;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var seekBar = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            seekBar.Max = VALOR_MAXIMO_SLIDER_TIEMPO;

            var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            var txtCancion = View.FindViewById<TextView>(Resource.Id.txtNombreCancion);
            var txtArtista = View.FindViewById<TextView>(Resource.Id.txtNombreArtista);
            var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
            var buttonRepetir = View.FindViewById<ImageButton>(Resource.Id.ibtnRepetir);
            var buttonSiguiente = View.FindViewById<ImageButton>(Resource.Id.ibtnSiguiente);
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            var buttonAnterior = View.FindViewById<ImageButton>(Resource.Id.ibtnAnterior);
            var buttonBarajar = View.FindViewById<ImageButton>(Resource.Id.ibtnBarajar);
            seekBarTiempo.Max = VALOR_MAXIMO_SLIDER_TIEMPO;
            seekBarTiempo.StartTrackingTouch += SeekBarTiempo_StartTrackingTouch;
            seekBarTiempo.StopTrackingTouch += SeekBarTiempo_StopTrackingTouch;
            buttonLike.Click += ButtonLike_Click;
            buttonRepetir.Click += ButtonRepetir_Click;
            buttonSiguiente.Click += ButtonSiguiente_Click;
            buttonReproducir.Click += ButtonReproducir_Click;
            buttonAnterior.Click += ButtonAnterior_Click;
            buttonBarajar.Click += ButtonBarajar_Click;
        }

        private void SeekBarTiempo_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            ActualizadorDeSlider.Enabled = true;
            var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            var longitud = Reproductor.Duration;
            var miliSegundos = (longitud * seekBarTiempo.Progress) / VALOR_MAXIMO_SLIDER_TIEMPO;
            Reproductor.SeekTo(miliSegundos);
            CancionCorriendo = true;
        }

        private void SeekBarTiempo_StartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e)
        {
            ActualizadorDeSlider.Enabled = false;
        }

        private void ButtonLike_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();

        }


        private void ActualizadorDeSlider_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //var txtTiempoActual = View.FindViewById<TextView>(Resource.Id.txtDuracionActual);
            //var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            //var longitud = Reproductor.Duration;
            //var posicion = Reproductor.CurrentPosition;
            //seekBarTiempo.Progress = (posicion * VALOR_MAXIMO_SLIDER_TIEMPO) / longitud;
            //if(seekBarTiempo.Progress == seekBarTiempo.Max)
            //{
            //    ActualizadorDeSlider.Enabled = false;
            //    Player_Completion();
            //    ActualizadorDeSlider.Enabled = true;
            //}
            //var minutos = posicion / 60000;
            //var segundos = posicion / 1000 % 60;
            //txtTiempoActual.SetText(System.String.Format("{0}:{1:D2}", minutos, segundos), TextView.BufferType.Normal);
        }

        private List<Cancion> _cancionesOrdenadas;
        private void ButtonBarajar_Click(object sender, EventArgs e)
        {
            var buttonBarajar = View.FindViewById<ImageButton>(Resource.Id.ibtnBarajar);
            if (AleatorizarActivado)
            {
                buttonBarajar.SetBackgroundColor(Color.Transparent);
                AleatorizarActivado = false;
                if (Canciones != null)
                {
                    List<Cancion> cancionesNuevas = new List<Cancion>();
                    foreach (Cancion cancion in Canciones)
                    {
                        if (!_cancionesOrdenadas.Contains(cancion))
                        {
                            cancionesNuevas.Add(cancion);
                        }
                    }
                    Canciones = _cancionesOrdenadas.Concat(cancionesNuevas).ToList();
                }
            }
            else
            {
                buttonBarajar.SetBackgroundColor(Color.LightGreen);
                AleatorizarActivado = true;
                if (Canciones != null)
                {
                    _cancionesOrdenadas = new List<Cancion>(Canciones);

                    var listaIndices = new List<int>();
                    for (int i = 0; i < Canciones.Count; i++)
                    {
                        listaIndices.Add(i);
                    }

                    List<Cancion> cancionesAleatorizadas = new List<Cancion>();
                    for (int i = 0; i < Canciones.Count; i++)
                    {
                        int random = new System.Random().Next(0, listaIndices.Count);
                        int indiceAleatorio = listaIndices[random];
                        listaIndices.RemoveAt(random);
                        cancionesAleatorizadas.Add(Canciones[indiceAleatorio]);
                    }

                    Canciones = cancionesAleatorizadas;
                }
            }
        }

        private void ButtonRepetir_Click(object sender, EventArgs e)
        {
            var buttonRepetir = View.FindViewById<ImageButton>(Resource.Id.ibtnRepetir);
            if (RepetirCancionActivado)
            {
                RepetirCancionActivado = false;
                buttonRepetir.SetBackgroundColor(Color.Transparent);
            }
            else
            {
                RepetirCancionActivado = true;
                buttonRepetir.SetBackgroundColor(Color.LightGreen);
            }
        }

        private void ButtonAnterior_Click(object sender, EventArgs e)
        {
            ReproducirCancionAnterior();
        }

        private void ButtonSiguiente_Click(object sender, EventArgs e)
        {
            ReproducirSiguienteCancion();
        }

        private void ButtonReproducir_Click(object sender, EventArgs e)
        {
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            if (Reproductor.IsPlaying)
            {
                Reproductor.Pause();
                ActualizadorDeSlider.Enabled = false;
                buttonReproducir.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_play));
            }
            else
            {
                Reproductor.Start();
                ActualizadorDeSlider.Enabled = true;
                buttonReproducir.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_pausa));
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.fragment_reproductor, container, false);
        }

        private void Reproducir(Cancion cancion)
        {
            DesactivarControles();
            byte[] archivo = null;
            bool huboExcepcion = false;
            if (!UtileriasDeArchivos.CancionYaDescargada(cancion.IdArchivo))
            {
                APIGatewayService api = new APIGatewayService();
                try
                {
                    archivo = api.DescargarArchivoPorId(cancion.IdArchivo);
                }
                catch (System.Exception)
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
                if(cancion.Artistas.FirstOrDefault() != null)
                {
                    txtArtista.Text = cancion.Artistas.FirstOrDefault().Nombre;
                }
                var txtTiempoTotal = View.FindViewById<TextView>(Resource.Id.txtDuracionTotal);
                TimeSpan tiempoActual = TimeSpan.FromMilliseconds(Reproductor.Duration);
                txtTiempoTotal.Text = System.String.Format("{0}:{1:D2}", tiempoActual.Minutes, tiempoActual.Seconds);
                try
                {
                    Reproductor.Reset();
                    Java.IO.File archivoTemporal = Java.IO.File.CreateTempFile(cancion.Id, "mp3", Context.CacheDir);
                    archivoTemporal.DeleteOnExit();
                    Java.IO.FileOutputStream outputStream = new Java.IO.FileOutputStream(archivoTemporal);
                    outputStream.Write(archivo);
                    outputStream.Close();
                    Java.IO.FileInputStream fis = new Java.IO.FileInputStream(archivoTemporal);
                    Reproductor.SetDataSource(fis.FD);
                    Reproductor.Prepare();
                    Reproductor.Start();
                    CancionCorriendo = true;
                    
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(View.Context, e.Message, ToastLength.Long).Show();
                }

                //AgregarCancionAHistorial(cancion.Id);
            }
            ActivarControles();
            ActualizadorDeSlider.Enabled = true;
        }

        private void DesactivarControles()
        {
            var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
            var buttonRepetir = View.FindViewById<ImageButton>(Resource.Id.ibtnRepetir);
            var buttonSiguiente = View.FindViewById<ImageButton>(Resource.Id.ibtnSiguiente);
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            var buttonAnterior = View.FindViewById<ImageButton>(Resource.Id.ibtnAnterior);
            var buttonBarajar = View.FindViewById<ImageButton>(Resource.Id.ibtnBarajar);
            var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            buttonLike.Enabled = false;
            buttonRepetir.Enabled = false;
            buttonSiguiente.Enabled = false;
            buttonReproducir.Enabled = false;
            buttonAnterior.Enabled = false;
            buttonBarajar.Enabled = false;
            seekBarTiempo.Enabled = false;
        }
        private void ActivarControles()
        {
            var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
            var buttonRepetir = View.FindViewById<ImageButton>(Resource.Id.ibtnRepetir);
            var buttonSiguiente = View.FindViewById<ImageButton>(Resource.Id.ibtnSiguiente);
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            var buttonAnterior = View.FindViewById<ImageButton>(Resource.Id.ibtnAnterior);
            var buttonBarajar = View.FindViewById<ImageButton>(Resource.Id.ibtnBarajar);
            var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            seekBarTiempo.Enabled = true;
            buttonLike.Enabled = true;
            buttonRepetir.Enabled = true;
            buttonSiguiente.Enabled = true;
            buttonReproducir.Enabled = true;
            buttonAnterior.Enabled = true;
            buttonBarajar.Enabled = true;
            seekBarTiempo.Enabled = true;
        }

        private void Player_Completion()
        {
            if (CancionCorriendo)
            {
                CancionCorriendo = false;
                ReproducirSiguienteCancion();
            }
        }

        private void ReproducirSiguienteCancion()
        {
            Reproductor.Stop();
            int siguienteIndiceActual = IndiceActual + 1;
            if (Canciones != null)
            {
                if (siguienteIndiceActual <= Canciones.Count - 1)
                {
                    IndiceActual++;
                    Reproducir(Canciones[IndiceActual]);
                }
                else if (RepetirCancionActivado)
                {
                    IndiceActual = 0;
                    Reproducir(Canciones[IndiceActual]);
                }
            }
        }

        private void ReproducirCancionAnterior()
        {
            Reproductor.Stop();
            if (IndiceActual - 1 >= 0 && Canciones.Count > 0)
            {
                IndiceActual--;
                Reproducir(Canciones[IndiceActual]);
            }
        }

        public void ReproducirLista(List<Cancion> lista, int indice = 0)
        {
            Canciones = lista;
            IndiceActual = indice;
            ListasFragment = new ListasFragment(lista, this, Usuario, CambiarContenido);
            //FragmentManager.BeginTransaction().Replace(Resource.Id.listViewCancionesEnReproduccion, ListasFragment).Commit();
            //ChildFragmentManager.BeginTransaction().Replace(Resource.Id.listViewCancionesEnReproduccion, ListasFragment).Commit();
            Reproducir(Canciones[IndiceActual]);
        }
        
        public void CerrarTodo()
        {
            Reproductor.Stop();
            ActualizadorDeSlider.Stop();
        }

        public void AñadirAlFinal(Cancion cancion)
        {
            if (Canciones != null)
            {
                Canciones.Add(cancion);
            }
            else
            {
                Canciones = new List<Cancion>();
                Canciones.Add(cancion);
            }
        }

        public void AñadirSiguiente(Cancion cancion)
        {
            if (Canciones != null)
            {
                if (Canciones.Count == 0)
                {
                    Canciones.Add(cancion);
                    IndiceActual = 0;
                }
                else
                {
                    Canciones.Insert(IndiceActual + 1, cancion);
                }
            }
            else
            {
                Canciones = new List<Cancion>();
                Canciones.Add(cancion);
                IndiceActual = 0;
            }
        }
    }
}