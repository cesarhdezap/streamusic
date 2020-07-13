using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util.Logging;
using Logica;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using Logica.Utilerias;
using StreamusicClientAndroid.Interfacez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

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
        Android.OS.Handler Handler;
        Timer ActualizadorDeInterfaz = new Timer();
        bool TokenDeCancelacion;
        bool CancionCorriendo = false;
        bool ActualizadorCorriendo = false;
        bool ActualizadorFinalizado = false;
        ListasFragment ListasFragment;


        const int VALOR_MAXIMO_SLIDER_TIEMPO = 1000;
        ListaDeReproduccion HistorialDeReproduccion;

        public ReproductorFragment(Usuario usuario, ICambiarContenido cambiarContenido)
        {
            CambiarContenido = cambiarContenido;
            Usuario = usuario;
            Handler = new Android.OS.Handler(Looper.MainLooper);
            ActualizadorDeInterfaz.Elapsed += PosterAinteraz;
            ActualizadorDeInterfaz.Enabled = true;
            ActualizadorDeInterfaz.Interval = 500;
        }

        private void PosterAinteraz(object sender, ElapsedEventArgs e)
        {
            Action action = new Action(ActualizadorDeSlider_Elapsed);
            Handler.PostDelayed(action, 1000);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var seekBar = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            seekBar.Max = VALOR_MAXIMO_SLIDER_TIEMPO;

            var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            var txtCancion = View.FindViewById<TextView>(Resource.Id.txtNombreCancion);
            var txtArtista = View.FindViewById<TextView>(Resource.Id.txtNombreArtista);
            var txtTiempoActualDeCancion = View.FindViewById<TextView>(Resource.Id.txtDuracionActual);
            var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
            var buttonRepetir = View.FindViewById<ImageButton>(Resource.Id.ibtnRepetir);
            var buttonSiguiente = View.FindViewById<ImageButton>(Resource.Id.ibtnSiguiente);
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            var buttonAnterior = View.FindViewById<ImageButton>(Resource.Id.ibtnAnterior);
            var buttonBarajar = View.FindViewById<ImageButton>(Resource.Id.ibtnBarajar);
            seekBarTiempo.Max = VALOR_MAXIMO_SLIDER_TIEMPO;
            seekBarTiempo.StartTrackingTouch += SeekBarTiempo_StartTrackingTouch;
            seekBarTiempo.StopTrackingTouch += SeekBarTiempo_StopTrackingTouch;
            txtArtista.Text = string.Empty;
            txtCancion.Text = string.Empty;
            txtTiempoActualDeCancion.Text = "0:00";
            buttonLike.Click += ButtonLike_Click;
            buttonRepetir.Click += ButtonRepetir_Click;
            buttonSiguiente.Click += ButtonSiguiente_Click;
            buttonReproducir.Click += ButtonReproducir_Click;
            buttonAnterior.Click += ButtonAnterior_Click;
            buttonBarajar.Click += ButtonBarajar_Click;
        }

        private void SeekBarTiempo_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            ActualizadorCorriendo = true;
            var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
            var longitud = Reproductor.Duration;
            var miliSegundos = (longitud * seekBarTiempo.Progress) / VALOR_MAXIMO_SLIDER_TIEMPO;
            Reproductor.SeekTo(miliSegundos);
            CancionCorriendo = true;
        }

        private void SeekBarTiempo_StartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e)
        {
            ActualizadorCorriendo = false;
        }

        private void ButtonLike_Click(object sender, EventArgs e)
        {
            try
            {
                Canciones[IndiceActual].CargarMetadatosDeLaCancion(Usuario.Id);
            }
            catch (Exception)
            {
                Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                return;
            }

            APIGatewayService api = new APIGatewayService();
            if (Canciones[IndiceActual].Metadatos == null)
            {
                bool resultado = false;
                try
                {
                    resultado = api.CrearNuevoMetadato(Usuario.Id, Canciones[IndiceActual].Id);
                }
                catch (Exception)
                {
                    Toast.MakeText(View.Context, "No se pudo registrar el MeGusta", ToastLength.Long).Show();
                }

                if (!resultado)
                {
                    Toast.MakeText(View.Context, "No se pudo registrar el MeGusta", ToastLength.Long).Show();
                }
                else
                {
                    Canciones[IndiceActual].CargarMetadatosDeLaCancion(Usuario.Id);
                }
            }

            if (Canciones[IndiceActual].Metadatos != null)
            {
                if (Canciones[IndiceActual].Metadatos.MeGusta)
                {
                    try
                    {
                        api.ActualizarMeGustaAMetadato(Canciones[IndiceActual].Metadatos.Id, false);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                    }

                    Canciones[IndiceActual].EliminarArchivoDeCancion();
                }
                else
                {
                    try
                    {
                        api.ActualizarMeGustaAMetadato(Canciones[IndiceActual].Metadatos.Id, true);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                    }
                    Canciones[IndiceActual].DescargarArchivoDeCancion();
                }

                try
                {
                    Canciones[IndiceActual].CargarMetadatosDeLaCancion(Usuario.Id);
                }
                catch (Exception)
                {
                    Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                }

            }

            var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
            if (Canciones[IndiceActual].Metadatos.MeGusta)
            {
                buttonLike.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_like));
            }
            else
            {
                buttonLike.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_dislike));
            }
        }


        private void ActualizadorDeSlider_Elapsed()
        {
            if (ActualizadorDeInterfaz.Enabled)
            {
                var txtTiempoActual = View.FindViewById<TextView>(Resource.Id.txtDuracionActual);
                var seekBarTiempo = View.FindViewById<SeekBar>(Resource.Id.seekBarTiempo);
                var longitud = Reproductor.Duration;
                var posicion = Reproductor.CurrentPosition;
                if (longitud != 0)
                {
                    seekBarTiempo.Progress = (posicion * VALOR_MAXIMO_SLIDER_TIEMPO) / longitud;
                }
                if (seekBarTiempo.Progress == seekBarTiempo.Max)
                {
                    ActualizadorCorriendo = false;
                    Player_Completion();
                    ActualizadorCorriendo = true;
                }
                var minutos = posicion / 60000;
                var segundos = posicion / 1000 % 60;
                txtTiempoActual.SetText(System.String.Format("{0}:{1:D2}", minutos, segundos), TextView.BufferType.Normal);
            }
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
            var buttonSiguiente = View.FindViewById<ImageButton>(Resource.Id.ibtnSiguiente);
            buttonSiguiente.SetBackgroundColor(Color.LightGreen);
            ReproducirCancionAnterior();
            buttonSiguiente.SetBackgroundColor(Color.Transparent);
        }

        private void ButtonSiguiente_Click(object sender, EventArgs e)
        {
            var buttonAnterior = View.FindViewById<ImageButton>(Resource.Id.ibtnAnterior);
            buttonAnterior.SetBackgroundColor(Color.LightGreen);
            ReproducirSiguienteCancion();
            buttonAnterior.SetBackgroundColor(Color.Transparent);
        }

        private void ButtonReproducir_Click(object sender, EventArgs e)
        {
            var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
            if (Reproductor.IsPlaying)
            {
                Reproductor.Pause();
                ActualizadorCorriendo = false;
                buttonReproducir.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_play));
            }
            else
            {
                Reproductor.Start();
                ActualizadorCorriendo = true;
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
            Action action = new Action(DesactivarControles);
            Handler.Post(action);
            byte[] archivo = null;
            bool huboExcepcion = false;
            APIGatewayService api = new APIGatewayService();
            if (!UtileriasDeArchivos.CancionYaDescargada(cancion.IdArchivo))
            {
                try
                {
                    archivo = api.DescargarArchivoPorId(cancion.IdArchivo);
                    cancion.CargarMetadatosDeLaCancion(Usuario.Id);
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

                if (Canciones[IndiceActual].Metadatos == null)
                {
                    bool resultado = false;
                    try
                    {
                        resultado = api.CrearNuevoMetadato(Usuario.Id, Canciones[IndiceActual].Id);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(View.Context, "No se pudo registrar el MeGusta", ToastLength.Long).Show();
                    }

                    if (!resultado)
                    {
                        Toast.MakeText(View.Context, "No se pudo registrar el MeGusta", ToastLength.Long).Show();
                    }
                    else
                    {
                        Canciones[IndiceActual].CargarMetadatosDeLaCancion(Usuario.Id);
                    }
                }
                var buttonLike = View.FindViewById<ImageButton>(Resource.Id.ibtnLike);
                if (cancion.Metadatos.MeGusta)
                {
                    buttonLike.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_like));
                }
                else
                {
                    buttonLike.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_dislike));
                }
                var buttonReproducir = View.FindViewById<ImageButton>(Resource.Id.ibtnReproducir);
                buttonReproducir.SetImageDrawable(View.Context.GetDrawable(Resource.Drawable.ic_ss_pausa));
                var txtCancion = View.FindViewById<TextView>(Resource.Id.txtNombreCancion);
                txtCancion.Text = cancion.Nombre;
                var txtArtista = View.FindViewById<TextView>(Resource.Id.txtNombreArtista);
                if (cancion.Artistas.FirstOrDefault() != null)
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

                AgregarCancionAHistorial(cancion.Id);
            }
            action = new Action(ActivarControles);
            Handler.Post(action);
            ActualizadorCorriendo = true;
        }
        private void AgregarCancionAHistorial(string idCancion)
        {
            APIGatewayService api = new APIGatewayService();
            HistorialDeReproduccion = ObtenerHistorialDeReproduccion();
            if (HistorialDeReproduccion == null)
            {
                Toast.MakeText(View.Context, "Lo sentimos ocurrio un error y no se puede recuperar su historial de canciones, intente mas tarde.", ToastLength.Long).Show();
            }
            else
            {
                if (HistorialDeReproduccion.IdsCanciones.Contains(idCancion))
                {
                    HistorialDeReproduccion.IdsCanciones.Remove(idCancion);
                    HistorialDeReproduccion.IdsCanciones.Insert(0, idCancion);

                }
                else
                {
                    HistorialDeReproduccion.IdsCanciones.Insert(0, idCancion);
                }

                try
                {
                    if (!api.ActualizarListaDeReproduccion(HistorialDeReproduccion.Id, HistorialDeReproduccion))
                    {
                        Toast.MakeText(View.Context, "Se produjo un error al agregar esta cancion al historial de reproduccion", ToastLength.Long).Show();
                    }
                }
                catch (Exception)
                {
                    Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                }

            }

        }

        private ListaDeReproduccion ObtenerHistorialDeReproduccion()
        {
            List<ListaDeReproduccion> listas = new List<ListaDeReproduccion>();
            APIGatewayService api = new APIGatewayService();

            try
            {
                listas = api.ObtenerTodasLasListasPorIdUsuario(Usuario.Id);
            }
            catch (Exception)
            {
                Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                listas = null;
            }
            if (listas != null)
            {

                HistorialDeReproduccion = listas.FirstOrDefault(l => l.EsHistorialDeReproduccion == true);

                if (HistorialDeReproduccion == null)
                {
                    if (!CrearNuevoHistorialReproduccion())
                    {
                        Toast.MakeText(View.Context, "Lo sentimos ocurrio un error y no se puede crear un nuevo historial de canciones, intente mas tarde.", ToastLength.Long).Show();
                    }
                    else
                    {
                        try
                        {
                            listas = api.ObtenerTodasLasListasPorIdUsuario(Usuario.Id);
                        }
                        catch (Exception)
                        {
                            Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                            listas = new List<ListaDeReproduccion>();
                        }

                        HistorialDeReproduccion = listas.FirstOrDefault(l => l.EsHistorialDeReproduccion == true);
                    }
                }
            }
            else
            {
                if (!CrearNuevoHistorialReproduccion())
                {
                    Toast.MakeText(View.Context, "Lo sentimos ocurrio un error y no se puede crear un nuevo historial de canciones, intente mas tarde.", ToastLength.Long).Show();
                }
                else
                {
                    try
                    {
                        listas = api.ObtenerTodasLasListasPorIdUsuario(Usuario.Id);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
                        listas = new List<ListaDeReproduccion>();
                    }

                    HistorialDeReproduccion = listas.FirstOrDefault(l => l.EsHistorialDeReproduccion == true);
                }
            }
            return HistorialDeReproduccion;
        }

        public bool CrearNuevoHistorialReproduccion()
        {
            bool resultado = false;
            APIGatewayService api = new APIGatewayService();

            try
            {
                resultado = api.CrearNuevaListaDeReproduccion(new ListaDeReproduccion
                {
                    Nombre = "HistorialDeReproduccion",
                    Descripcion = string.Empty,
                    IdUsuario = Usuario.Id,
                    IdsCanciones = new List<string>(),
                    EsHistorialDeReproduccion = true
                });
            }
            catch (Exception)
            {
                Toast.MakeText(View.Context, "Error al conectarse al servidor. Intente mas tarde.", ToastLength.Long).Show();
            }


            return resultado;
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
            seekBarTiempo.Enabled = false;
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
            int siguienteIndiceActual = IndiceActual + 1;
            if (Canciones != null)
            {
                if (siguienteIndiceActual <= Canciones.Count - 1)
                {
                    Reproductor.Stop();
                    IndiceActual++;
                    Reproducir(Canciones[IndiceActual]);
                }
                else if (RepetirCancionActivado)
                {
                    Reproductor.Stop();
                    IndiceActual = 0;
                    Reproducir(Canciones[IndiceActual]);
                }
            }
        }

        private void ReproducirCancionAnterior()
        {
            if (IndiceActual - 1 >= 0 && Canciones.Count > 0)
            {
                Reproductor.Stop();
                IndiceActual--;
                Reproducir(Canciones[IndiceActual]);
            }
            else if (RepetirCancionActivado)
            {
                   Reproductor.Stop();
                IndiceActual = Canciones.Count - 1;
                Reproducir(Canciones[IndiceActual]);
            }
        }

        public void ReproducirLista(List<Cancion> lista, int indice = 0)
        {
            Canciones = lista;
            IndiceActual = indice;
            ListasFragment = new ListasFragment(lista, this, Usuario, CambiarContenido);
            ChildFragmentManager.BeginTransaction().Replace(Resource.Id.listViewCancionesEnReproduccion, ListasFragment).Commit();
            Reproducir(Canciones[IndiceActual]);
        }

        public void CerrarTodo()
        {
            Reproductor.Stop();
            ActualizadorFinalizado = true;
            ActualizadorDeInterfaz.Enabled = false;
            ActualizadorDeInterfaz.Stop();
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