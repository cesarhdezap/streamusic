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

namespace StreamusicClientAndroid
{
    [Activity(Label = "PaginaPrincipalActivity")]
    public class PaginaPrincipalActivity : AppCompatActivity
    {
        Usuario Usuario;
        Android.Support.V4.App.Fragment Reproductor;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_paginaprincipal);
            
            // Create your application here
            Reproductor = new ReproductorFragment();
            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));
            

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += (s, e) =>
            {
                CargarFragmento(e.Item.ItemId);
            };
        }
        
        void CargarFragmento(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_inicio:
                    fragment = new FragmentInicio(Usuario);
                    break;
                case Resource.Id.menu_listas:
                    fragment = new ListasFragment();
                    Toast.MakeText(this, "Listas", ToastLength.Short).Show();
                    break;
                case Resource.Id.menu_play:
                    Toast.MakeText(this, "Play", ToastLength.Short).Show();
                    fragment = Reproductor;
                    break;
            }

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
        }
    }


}