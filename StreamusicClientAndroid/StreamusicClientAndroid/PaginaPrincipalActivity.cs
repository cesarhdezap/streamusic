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

namespace StreamusicClientAndroid
{
    [Activity(Label = "PaginaPrincipalActivity",Theme ="@style/Theme.AppCompat,Light.NoActionBar")]
    public class PaginaPrincipalActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_paginaprincipal);

            // Create your application here
            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
        }
    }
}