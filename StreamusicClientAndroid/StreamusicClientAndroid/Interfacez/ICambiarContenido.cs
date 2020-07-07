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

namespace StreamusicClientAndroid.Interfacez
{
    public interface ICambiarContenido
    {
        public void CambiarContenido(Android.Support.V4.App.Fragment userControl);
        public void CambiarAReproductor();
    }
}