using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StreamusicClientAndroid
{
    [Activity(Label = "RegistroDeUsuarioActivity")]
    public class RegistroDeUsuarioActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_registrodeusuario);

            Button btnres = FindViewById<Button>(Resource.Id.ButtonRegistrarse);
            btnres.Click += ButtonRegistrarseOnClick;
        }

        private void ButtonRegistrarseOnClick(object sender, EventArgs eventArgs)
        {
            
        }
    }
}