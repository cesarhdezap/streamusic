using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using static Logica.Servicios.ServiciosDeEncriptacion;
using static Logica.Utilerias.UtileriasDeExcepciones;
using Logica.Clases;
using Logica.ServiciosDeComunicacion;
using Logica.Enumeradores;
using Android.Content;

namespace StreamusicClientAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        APIGatewayService APIGatewayService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            APIGatewayService = new APIGatewayService();

            Button btnis = FindViewById<Button>(Resource.Id.ButtonIniciarSesion);
            btnis.Click += ButtonRegistrarseOnClick;
            Button btreg = FindViewById<Button>(Resource.Id.ButtonRegistrarse);
            btreg.Click += ButtonRegistrarseOnClick;
        }

        private void ButtonRegistrarseOnClick(object sender, EventArgs eventArgs)
        {
            string usuario = FindViewById<EditText>(Resource.Id.EditTextUsuario).Text;
            string contraseña = FindViewById<EditText>(Resource.Id.EditTextContrseña).Text;
            contraseña = EncriptarCadena(contraseña);

            RespuestaLogin respuestaLogin = null;
            bool huboExcepcion = false;
            try
            {
                respuestaLogin = APIGatewayService.AutenticarUsuario(usuario, contraseña);
            }
            catch (AggregateException ex)
            {
                string mensaje = ObtenerMensajesDeAggregateException(ex);
                Toast.MakeText(ApplicationContext, mensaje, ToastLength.Long).Show();
                huboExcepcion = true;
            }

            if (respuestaLogin == null && !huboExcepcion)
            {
                Toast.MakeText(ApplicationContext, "No hay conexion con el servidor", ToastLength.Long).Show();
            }
            else if (!huboExcepcion)
            {
                if (respuestaLogin.Codigo == (int)CodigoAutenticacion.Autorizado)
                {
                    Toast.MakeText(ApplicationContext, "Permitido", ToastLength.Long).Show();
                    //PagePaginaPrincipal pagePaginaPrincipal = new PagePaginaPrincipal(ControladorPaginas, respuestaLogin.Usuario);
                    //ControladorPaginas.CambiarANuevaPage(pagePaginaPrincipal);
                }
                else
                {
                    Toast.MakeText(ApplicationContext, "Denegado", ToastLength.Long).Show();
                }
            }
        }

        private async void ButtonIniciarSesionOnClick(object sender, EventArgs eventArgs)
        {

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}