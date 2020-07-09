using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Security;
using Logica;
using Newtonsoft.Json;
using StreamusicClientAndroid.Interfacez;

namespace StreamusicClientAndroid
{
    [Activity]
    public class PaginaPrincipalActivity : AppCompatActivity, ICambiarContenido
    {
        Usuario Usuario;
        ReproductorFragment Reproductor;

        public void CambiarAReproductor()
        {
            EliminarFragmentosYOcultarReproductor();
            SupportFragmentManager.BeginTransaction().Show(Reproductor).Commit();
        }

        public void CambiarContenido(Android.Support.V4.App.Fragment fragment)
        {
            if (fragment is ReproductorFragment)
            {
                throw new NotImplementedException();
            }
            EliminarFragmentosYOcultarReproductor();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_paginaprincipal);

            Reproductor = new ReproductorFragment(Usuario, this);
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, Reproductor).Hide(Reproductor).Commit();

            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += (s, e) =>
            {
                CargarFragmento(e.Item.ItemId);
            };
        }

        void EliminarFragmentosYOcultarReproductor()
        {
            foreach (var fragm in SupportFragmentManager.Fragments)
            {
                if (fragm is ReproductorFragment)
                {
                    SupportFragmentManager.BeginTransaction().Hide(fragm).Commit();
                }
                else
                {
                    SupportFragmentManager.BeginTransaction().Remove(fragm).Commit();
                }
            }
        }

        void CargarFragmento(int id)
        {
            EliminarFragmentosYOcultarReproductor();

            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_inicio:
                    fragment = new FragmentInicio(Usuario, Reproductor, this);
                    SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
                    break;

                case Resource.Id.menu_listas:
                    fragment = new VerMisListasFragment(Usuario, Reproductor, this);
                    SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
                    break;

                case Resource.Id.menu_play:
                    SupportFragmentManager.BeginTransaction().Show(Reproductor).Commit();
                    break;
            }
        }

        public override void OnBackPressed()
        {
            Reproductor.CerrarTodo();
            Finish();
        }
    }

}