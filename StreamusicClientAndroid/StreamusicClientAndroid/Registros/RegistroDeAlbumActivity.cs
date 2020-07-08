using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Logica;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using Newtonsoft.Json;
using Plugin.Media;
using static Logica.Servicios.ServiciosDeValidacion;
using static Logica.Utilerias.UtileriasDeExcepciones;

namespace StreamusicClientAndroid.Registros
{
    [Activity(Label = "RegistroDeAlbumActivity")]
    public class RegistroDeAlbumActivity : Activity
    {
        byte[] Arreglo;
        Usuario Usuario;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_registrodealbum);

            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            Button registro = FindViewById<Button>(Resource.Id.ButtonRegistro);
            registro.Click += ButtonRegistroOnClick;

            Button cargarImagenArtista = FindViewById<Button>(Resource.Id.ButtonSubirImagen);
            cargarImagenArtista.Click += ButtonCargarImagen_Click;
        }

        private void ButtonCargarImagen_Click(object sender, EventArgs e)
        {
            CargarImagen();
        }

        private void ButtonRegistroOnClick(object sender, EventArgs e)
        {
            bool resultado = false;
            bool huboExcepcion = false;
            APIGatewayService service = new APIGatewayService();
            if (ValidarEntradas())
            {
                Album album = new Album
                {
                    Nombre = FindViewById<EditText>(Resource.Id.EditTextNombreAlbum).Text,
                    AñoDeLanzamiento = int.Parse(FindViewById<EditText>(Resource.Id.EditTextLanzamiento).Text),
                    CompañiaDiscografica = FindViewById<EditText>(Resource.Id.EditTextDiscografia).Text,
                    IdArtista = Usuario.IdArtista,
                    Ilustracion = Arreglo
                };
                try
                {
                    resultado = service.CrearAlbum(album);
                }
                catch (AggregateException ex)
                {
                    string mensaje = ObtenerMensajesDeAggregateException(ex);
                    Toast.MakeText(ApplicationContext, mensaje, ToastLength.Short).Show();
                    huboExcepcion = true;
                }

                if (!huboExcepcion && resultado)
                {
                    Toast.MakeText(ApplicationContext, "¡Registro de albúm Exitoso!", ToastLength.Short).Show();

                }
                else if (!resultado)
                {
                    Toast.MakeText(ApplicationContext, "Lo sentimos ocurrio un error inesperado en el servidor, ¡Porfavor intente mas tarde!", ToastLength.Short).Show();

                }
                else
                {
                    Toast.MakeText(ApplicationContext, "No hay conexion con el servidor", ToastLength.Short).Show();
                }

            }
            else
            {
                Toast.MakeText(ApplicationContext, "¡Ups!, algo salio mal porfavor verifica tus datos.", ToastLength.Short).Show();
            }
        }

        async void CargarImagen()
        {
            Button cargarImagenArtista = FindViewById<Button>(Resource.Id.ButtonSubirImagen);

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "La carga no es soportada en este dispositivo", ToastLength.Short).Show();
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40

            });

            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            ImageView imagenArtista = FindViewById<ImageView>(Resource.Id.ImageViewArtista);
            imagenArtista.SetImageBitmap(bitmap);
            Arreglo = imageArray;
        }

        private bool ValidarEntradas()
        {
            bool resultado = false;
            int contadorDeErrores = 0;
            string mensajeError = "";

            if (!ValidarNombre(FindViewById<EditText>(Resource.Id.EditTextNombreAlbum).Text))
            {
                mensajeError += "El nombre ingresado es invalido\n";
                contadorDeErrores++;
            }
            if (!ValidarNombre(FindViewById<EditText>(Resource.Id.EditTextDiscografia).Text))
            {
                mensajeError += "La compañia debe tener un largo de 6 a 255 caractéres\n";
                contadorDeErrores++;
            }
            if (!ValidarEntero(FindViewById<EditText>(Resource.Id.EditTextLanzamiento).Text))
            {
                mensajeError += "El año de lanzamiento debe contener solo numeros enteros\n";
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