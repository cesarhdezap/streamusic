using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Com.Obsez.Android.Lib.Filechooser;
using Java.Lang;
using Logica;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static Com.Obsez.Android.Lib.Filechooser.Listeners;
using static Logica.Servicios.ServiciosDeValidacion;
using static Logica.Utilerias.UtileriasDeExcepciones;
using static Logica.Utilerias.UtileriasDeArchivos;
using Logica.Enumeradores;
using System.Linq;
using Android.Support.V7.View.Menu;

namespace StreamusicClientAndroid.Registros
{
    [Activity(Label = "RegistroDeCancionActivity")]
    public class RegistroDeCancionActivity : AppCompatActivity
    {
        byte[] Arreglo;
        Usuario Usuario;
        string PATH;
        List<Album> Albums;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_registrodecancion);

            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            SetDataSpinner();

            Button registro = FindViewById<Button>(Resource.Id.ButtonRegistro);
            registro.Click += ButtonRegistroOnClick;

            Button cargarCancion = FindViewById<Button>(Resource.Id.ButtonSubirCancion);
            cargarCancion.Click += ButtonCargarCancion_Click;

        }

        private void ButtonCargarCancion_Click(object sender, EventArgs e)
        {
            CargarCancion();
        }

        private void ButtonRegistroOnClick(object sender, EventArgs e)
        {
            Cancion cancion = new Cancion();
            Album albumSeleccionado = new Album();
            APIGatewayService service = new APIGatewayService();
            
            Spinner spinnerGenero = FindViewById<Spinner>(Resource.Id.SpinnerGenero);
            Spinner spinnerAlbum = FindViewById<Spinner>(Resource.Id.SpinnerAlbum);

            if (ValidarEntradas())
            {
                int posicionSpinner = spinnerAlbum.SelectedItemPosition;

                albumSeleccionado = Albums[posicionSpinner];

               

                if (albumSeleccionado != null)
                {
                    cancion.Nombre = FindViewById<EditText>(Resource.Id.EditTextNombre).Text;
                    cancion.IdsArtistas.Add(Usuario.IdArtista);
                    cancion.IdAlbum = albumSeleccionado.Id;
                    cancion.Genero = ((GeneroMusical)spinnerGenero.SelectedItemPosition).ToString();
                    cancion.EsPublico = true;

                    var datosDelArchivo = LeerArchivoPorURL(PATH);

                    cancion.IdArchivo = service.CargarArchivos(datosDelArchivo);

                    if (cancion.IdArchivo != null && cancion.IdArchivo != string.Empty)
                    {
                        service.CrearCancion(cancion);
                        Toast.MakeText(ApplicationContext, "¡Registro Exitoso!", ToastLength.Short).Show();
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(ApplicationContext, "Error al subir el archivo. Intentelo mas tarde.", ToastLength.Short).Show();

                    }
                }
            }
            else
            {
                Toast.MakeText(ApplicationContext, "¡Ups!, algo salio mal porfavor verifica tus datos.", ToastLength.Short).Show();

            }
        }


        private void CargarCancion()
        {
           MostrarExploradorArchivos();

        }

        private void ArchivoSeleccionado()
        {
            if (PATH != null)
            {

                TextView ruta = FindViewById<TextView>(Resource.Id.TextViewPathCancion);

                ruta.Text += PATH;

                MediaMetadataRetriever mmr = new MediaMetadataRetriever();
                mmr.SetDataSource(PATH);
                string duracion = mmr.ExtractMetadata(MetadataKey.Duration);

                duracion = ConvertirDuracion(duracion);

                TextView TextDuracion = FindViewById<TextView>(Resource.Id.textViewDuracion);
                TextDuracion.Text += duracion;

                Arreglo = System.IO.File.ReadAllBytes(PATH);

            }
            else
            {
                Toast.MakeText(this, "No se selecciono ninguna canción", ToastLength.Short).Show();

            }
        }

        private string ConvertirDuracion(string duracion)
        {
            float dura = Float.ParseFloat(duracion);

            dura /= 60000;

            return dura.ToString();

        }

        private void MostrarExploradorArchivos()
        {

            ChooserDialog chooserDialog = new ChooserDialog(this).WithStringResources("Choose a file", "Choose", "Cancel").WithOptionStringResources("New folder", "Delete", "Cancel", "Ok").EnableOptions(true).DisplayPath(true).WithChosenListener((dir, dirFile) =>
              {
                  PATH = dir;
                  Toast.MakeText(this, (dirFile.IsDirectory ? "FOLDER: " : "FILE: ") + dir, ToastLength.Short).Show();
                  ArchivoSeleccionado();
                  
              }).Show();
        }

        private void SetDataSpinner()
        {
            Spinner spinnerGenero = FindViewById<Spinner>(Resource.Id.SpinnerGenero);
            Spinner spinnerAlbum = FindViewById<Spinner>(Resource.Id.SpinnerAlbum);
            Albums = CargarAlbumes();

            List<string> items = new List<string>();

            foreach (Album album in Albums)
            {
                items.Add(album.ToString());
            }

            var adapterAlbum = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);

            spinnerAlbum.Adapter = adapterAlbum;

            var itemsGenero = System.Enum.GetValues(typeof(GeneroMusical));
            var arrayForAdapter = itemsGenero.Cast<GeneroMusical>().Select(e => e.ToString()).ToArray();
            var adapterGenero = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, arrayForAdapter);

            spinnerGenero.Adapter = adapterGenero;
        }

        private List<Album> CargarAlbumes()
        {
            APIGatewayService service = new APIGatewayService();
            List<Album> albumes = service.ObtenerAlbumsPorIdArtista(Usuario.IdArtista);

            return albumes;
        }

        private bool ValidarEntradas()
        {
            Spinner spinnerGenero = FindViewById<Spinner>(Resource.Id.SpinnerGenero);
            Spinner spinnerAlbum = FindViewById<Spinner>(Resource.Id.SpinnerAlbum);


            bool resultado = false;
            int contadorDeErrores = 0;
            string mensajeError = "";

            if (PATH == null)
            {
                mensajeError += "No se ha cargado ninguna canción\n";
                contadorDeErrores++;
            }

            if (!ValidarNombre(FindViewById<EditText>(Resource.Id.EditTextNombre).Text))
            {
                mensajeError += "El nombre ingresado es invalido\n";
                contadorDeErrores++;
            }

            if(spinnerAlbum.SelectedItemPosition == -1)
            {
                mensajeError += "No se selecciono un álbum\n";
                contadorDeErrores++;
            }
            
            if (contadorDeErrores == 0)
            {
                resultado = true;
            }
            else
            {
                Android.App.AlertDialog.Builder alerta = new Android.App.AlertDialog.Builder(this);
                alerta.SetTitle("Alerta");
                alerta.SetMessage(mensajeError);
                alerta.SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    Toast.MakeText(ApplicationContext, "¡Ups!, algo salio mal porfavor verifica tus datos.", ToastLength.Short).Show();
                });

                Dialog dialog = alerta.Create();
                dialog.Show();
            }

            return resultado;
        }


    }
}