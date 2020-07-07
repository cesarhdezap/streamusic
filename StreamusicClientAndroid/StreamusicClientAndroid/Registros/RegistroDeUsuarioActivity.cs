using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Logica;
using Logica.ServiciosDeComunicacion;
using System;
using System.Linq;
using System.Collections.Generic;
using static Logica.Servicios.ServiciosDeValidacion;
using static Logica.Utilerias.UtileriasDeExcepciones;

namespace StreamusicClientAndroid
{
    [Activity(Label = "RegistroDeUsuarioActivity")]
    public class RegistroDeUsuarioActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_registrodeusuario);

            Button btnres = FindViewById<Button>(Resource.Id.ButtonRegistro);
            btnres.Click += ButtonRegistroOnClick;
        }

        private void ButtonRegistroOnClick(object sender, EventArgs eventArgs)
        {
            bool resultado = false;
            bool huboExcepcion = false;

            APIGatewayService service = new APIGatewayService();

            if (ValidarEntradas())
            {
                if (!UsuarioExistente(FindViewById<EditText>(Resource.Id.EditTextUsuario).Text)) {
                    Usuario usuario = new Usuario
                    {
                        NombreDeUsuario = FindViewById<EditText>(Resource.Id.EditTextUsuario).Text,
                        Contraseña = FindViewById<EditText>(Resource.Id.EditTextContraseña).Text,
                    };

                    try
                    {
                        resultado = service.CrearUsuario(usuario); //ex
                    }
                    catch (AggregateException ex)
                    {
                        string mensaje = ObtenerMensajesDeAggregateException(ex);
                        Toast.MakeText(ApplicationContext, mensaje, ToastLength.Short).Show();
                        huboExcepcion = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (!huboExcepcion && resultado)
                    {
                        Toast.MakeText(ApplicationContext, "¡Registro Exitoso!", ToastLength.Short).Show();
                        Intent intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);

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
                    Android.App.AlertDialog.Builder alerta = new Android.App.AlertDialog.Builder(this);
                    alerta.SetTitle("Alerta");
                    alerta.SetMessage("El nombre de usuario ya se encuentra ocupado");
                    alerta.SetPositiveButton("Aceptar", (senderAlert, args) =>
                    {
                        Toast.MakeText(ApplicationContext, "Porfavor selecciona otro nombre de usuario.", ToastLength.Short).Show();
                    });

                    Dialog dialog = alerta.Create();
                    dialog.Show();
                }
            }
            else
            {
                Toast.MakeText(ApplicationContext, "¡Ups!, algo salio mal porfavor verifica tus datos.", ToastLength.Short).Show();
            }
        }

        private bool ValidarEntradas()
        {
            bool resultado = false;
            int contadorDeErrores = 0;
            string mensajeError = "";

            if (!ValidarNombre(FindViewById<EditText>(Resource.Id.EditTextUsuario).Text))
            {
                mensajeError += "El nombre ingresado es invalido\n";
                contadorDeErrores++;
            }
            if (!ValidarContraseña(FindViewById<EditText>(Resource.Id.EditTextContraseña).Text))
            {
                mensajeError += "La contraseña debe tener un largo de 6 a 255 caractéres\n";
                contadorDeErrores++;
            }
            if (FindViewById<EditText>(Resource.Id.EditTextContraseña).Text != FindViewById<EditText>(Resource.Id.EditTextConfirmarContraseña).Text)
            {
                mensajeError += "Ambas contraseñas deben ser iguales\n";
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

        public bool UsuarioExistente(string nombre)
        {
            bool resultado = false;

            APIGatewayService service = new APIGatewayService();

            List<Usuario> usuariosRecuperados = service.ObtenerTodosLosUsuarios();

            if(usuariosRecuperados.Exists(usuario => usuario.NombreDeUsuario == nombre))
            {
                resultado = true;
            }

            return resultado;
        }


    }
}