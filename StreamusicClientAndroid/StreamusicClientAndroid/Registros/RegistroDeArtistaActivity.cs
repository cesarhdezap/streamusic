using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Plugin.Media;
using static Logica.Servicios.ServiciosDeValidacion;
using static Logica.Utilerias.UtileriasDeExcepciones;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using Logica;
using Android.Media;
using Newtonsoft.Json;

namespace StreamusicClientAndroid.Registros
{
    [Activity(Label = "RegistroDeArtistaActivity")]
    public class RegistroDeArtistaActivity : AppCompatActivity
    {
        byte[] Arreglo;
        Usuario Usuario;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_registrodeartista);

            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            Button registro = FindViewById<Button>(Resource.Id.ButtonRegistro);
            registro.Click += ButtonRegistroOnClick;

            Button cargarImagenArtista = FindViewById<Button>(Resource.Id. ButtonSubirImagen);
            cargarImagenArtista.Click += ButtonCargarImagen_Click;
        }

        private void ButtonCargarImagen_Click(object sender, EventArgs e)
        {
            CargarImagen();
        }

        private void ButtonRegistroOnClick(object sender, EventArgs e)
        {
            string resultado = null;
            bool resultadoActualizarUsuario = false;
            bool huboExcepcionRegistroArtista = false;

            APIGatewayService service = new APIGatewayService();

            if (ValidarEntradas())
            {
                Artista artista = new Artista
                {
                    Nombre = FindViewById<EditText>(Resource.Id.EditTextArtista).Text,
                    Descripcion = FindViewById<EditText>(Resource.Id.EditTextDescripcion).Text,
                    Ilustracion = Arreglo
                };

                try
                {
                    resultado = service.CrearArtista(artista);
                }
                catch (AggregateException ex)
                {
                    string mensaje = ObtenerMensajesDeAggregateException(ex);
                    Toast.MakeText(ApplicationContext, mensaje, ToastLength.Short).Show();
                    huboExcepcionRegistroArtista = true;
                }


                if (!huboExcepcionRegistroArtista && resultado != null)
                {
                    Usuario.IdArtista = resultado;
                    resultadoActualizarUsuario = service.ActualizarUsuarioAsync(Usuario.Id, Usuario);

                    if (resultadoActualizarUsuario)
                    {
                        Toast.MakeText(ApplicationContext, "¡Registro Exitoso!", ToastLength.Short).Show();

                        Intent intent = new Intent(this, typeof(PaginaPrincipalActivity));
                        intent.PutExtra("usuario", JsonConvert.SerializeObject(Usuario));
                        StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(ApplicationContext, "Lo sentimos ocurrio un error inesperado en el servidor, ¡Porfavor intente mas tarde!", ToastLength.Short).Show();
                    }
                }
                else if (resultado == null)
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
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                CompressionQuality = 40

            }) ;

            if (file == null)
            {
                Toast.MakeText(ApplicationContext, "No selecciono ninguna imagen", ToastLength.Short).Show();
                ImageView imagenAlbum = FindViewById<ImageView>(Resource.Id.ImageViewAlbum);
                imagenAlbum.SetImageBitmap(null);

            }
            else
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                ImageView imagenArtista = FindViewById<ImageView>(Resource.Id.ImageViewArtista);
                imagenArtista.SetImageBitmap(bitmap);
                Arreglo = imageArray;
            }
        }

        private bool ValidarEntradas()
        {
            bool resultado = false;
            int contadorDeErrores = 0;
            string mensajeError = "";

            if (!ValidarNombre(FindViewById<EditText>(Resource.Id.EditTextArtista).Text))
            {
                mensajeError += "El nombre ingresado es invalido\n";
                contadorDeErrores++;
            }
            if (!ValidarCadenaVacioPermitido(FindViewById<EditText>(Resource.Id.EditTextDescripcion).Text))
            {
                mensajeError += "La descripción debe tener un largo de 6 a 255 caractéres\n";
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