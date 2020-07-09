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
        FragmentInicio FragmentInicio;
        VerMisListasFragment VerMisListasFragment;

        public void CambiarAReproductor()
        {
            OcultarFragmentos();
            SupportFragmentManager.BeginTransaction().Show(Reproductor).Commit();
        }

        public void CambiarAInicio()
        {

        }

        public void CambiarContenido(Android.Support.V4.App.Fragment fragment)
        {
            if (fragment is ReproductorFragment)
            {
                throw new NotImplementedException();
            }
            OcultarFragmentos();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
            SupportFragmentManager.BeginTransaction().Show(fragment).Commit();

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_paginaprincipal);
            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));
            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            Reproductor = new ReproductorFragment(Usuario, this);
            FragmentInicio = new FragmentInicio(Usuario, Reproductor, this);
            VerMisListasFragment = new VerMisListasFragment(Usuario, Reproductor, this);
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, Reproductor).Hide(Reproductor).Commit();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, FragmentInicio).Hide(Reproductor).Commit();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, VerMisListasFragment).Hide(Reproductor).Commit();


            bottomNavigation.NavigationItemSelected += (s, e) =>
            {
                CargarFragmento(e.Item.ItemId);
            };
        }

        void OcultarFragmentos()
        {
            foreach (var fragm in SupportFragmentManager.Fragments)
            {

                    SupportFragmentManager.BeginTransaction().Hide(fragm).Commit();

            }
        }

        void CargarFragmento(int id)
        {
            OcultarFragmentos();

            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_inicio:
                    SupportFragmentManager.BeginTransaction().Show(FragmentInicio).Commit();
                    break;

                case Resource.Id.menu_listas:
                    SupportFragmentManager.BeginTransaction().Show(VerMisListasFragment).Commit();
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