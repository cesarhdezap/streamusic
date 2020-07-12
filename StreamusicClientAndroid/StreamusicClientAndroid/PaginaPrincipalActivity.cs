using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Logica;
using Newtonsoft.Json;
using StreamusicClientAndroid.Interfacez;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Support.Design.Internal;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using Android.Runtime;
using StreamusicClientAndroid.Registros;
using Logica.ServiciosDeComunicacion;
using System.Collections.Generic;
using Logica.Clases;

namespace StreamusicClientAndroid
{
    [Activity(Theme ="@style/MyTheme")]
    public class PaginaPrincipalActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, ICambiarContenido
    {
        Usuario Usuario;
        ReproductorFragment Reproductor;
        private Android.Support.V4.Widget.DrawerLayout drawerLayout;
        private NavigationView navView;


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
            base.SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_paginaprincipal);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this,drawerLayout,toolbar,Resource.String.navigation_drawer_open,Resource.String.navigation_drawer_close);
            drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();
            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navView.SetNavigationItemSelectedListener(this);

            Usuario = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            Reproductor = new ReproductorFragment(Usuario, this);
            base.SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, Reproductor).Hide(Reproductor).Commit();


            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += (s, e) =>
            {
                CargarFragmento(e.Item.ItemId);
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.toolbar)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
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
                    base.SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
                    break;

                case Resource.Id.menu_listas:
                    fragment = new VerMisListasFragment(Usuario, Reproductor, this);
                    base.SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, fragment).Commit();
                    break;

                case Resource.Id.menu_play:
                    SupportFragmentManager.BeginTransaction().Show(Reproductor).Commit();
                    break;
            }
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);

            }
            else
            {
                Reproductor.CerrarTodo();
                Finish();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_artista)
            {
                if(Usuario.IdArtista == null || Usuario.IdArtista == string.Empty)
                {
                    Intent intent = new Intent(this, typeof(RegistroDeArtistaActivity));
                    intent.PutExtra("usuario", JsonConvert.SerializeObject(Usuario));
                    StartActivity(intent);
                }
                else if(Usuario.IdArtista != null || Usuario.IdArtista == string.Empty )
                {
                    Toast.MakeText(ApplicationContext, "¡Usted ya registro al artista!", ToastLength.Short).Show();
                }
            }
            else if (id == Resource.Id.nav_album)
            {
                if (Usuario.IdArtista == null || Usuario.IdArtista == string.Empty)
                {
                    Toast.MakeText(ApplicationContext, "¡Debe registrar al artista primero!", ToastLength.Short).Show();
                }
                else if (Usuario.IdArtista != null || Usuario.IdArtista == string.Empty)
                {
                    Intent intent = new Intent(this, typeof(RegistroDeAlbumActivity));
                    intent.PutExtra("usuario", JsonConvert.SerializeObject(Usuario));
                    StartActivity(intent);
                }
            }
            else if (id == Resource.Id.nav_cancion)
            {
                if(Usuario.IdArtista != null && Usuario.IdArtista != string.Empty)
                {
                    APIGatewayService api = new APIGatewayService();
                    List<Album> albums;
                    bool huboExcepcion = false;
                    try
                    {
                        albums = api.ObtenerAlbumsPorIdArtista(Usuario.IdArtista);
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(ApplicationContext, "Error al cargar albums.", ToastLength.Short).Show();
                        huboExcepcion = true;
                        albums = new List<Album>();
                    }

                    if (!huboExcepcion && albums.Count > 0)
                    {
                        Intent intent = new Intent(this, typeof(RegistroDeCancionActivity));
                        intent.PutExtra("usuario", JsonConvert.SerializeObject(Usuario));
                        StartActivity(intent);
                    }
                    else if (!huboExcepcion)
                    {
                        Toast.MakeText(ApplicationContext, "Debe registrar al menos un album.", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(ApplicationContext, "Debe registrarse como artista.", ToastLength.Short).Show();
                }
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}