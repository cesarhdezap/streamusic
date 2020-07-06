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
using Newtonsoft.Json;

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
            btnis.Click += ButtonIniciarSesionOnClick;
            Button btreg = FindViewById<Button>(Resource.Id.ButtonRegistrarse);
            btreg.Click += ButtonRegistrarseOnClick;
        }

        private void ButtonRegistrarseOnClick(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(RegistroDeUsuarioActivity));
        }

        private void ButtonIniciarSesionOnClick(object sender, EventArgs eventArgs)
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
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Short).Show();
                huboExcepcion = true;
            }

            if (respuestaLogin == null && !huboExcepcion)
            {
                Toast.MakeText(ApplicationContext, "Hubo un error con el servidor. Intente más tarde.", ToastLength.Short).Show();
            }
            else if (!huboExcepcion)
            {
                if (respuestaLogin.Codigo == (int)CodigoAutenticacion.Autorizado)
                {
                    Intent intent = new Intent(this, typeof(PaginaPrincipalActivity));
                    intent.PutExtra("usuario", JsonConvert.SerializeObject(respuestaLogin.Usuario));
                    StartActivity(intent);
                    
                    
                }
                else
                {
                    Toast.MakeText(ApplicationContext, "Denegado", ToastLength.Short).Show();
                }
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}