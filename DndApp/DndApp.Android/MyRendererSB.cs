using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DndApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DndApp.Models.MySearch), typeof(MyRendererSB))]
namespace DndApp.Droid
{
    public class MyRendererSB : SearchBarRenderer
    {
        public MyRendererSB(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);

            var iconSearch = Control?.FindViewById(Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
            (iconSearch as ImageView)?.SetColorFilter(Color.White.ToAndroid());

            var iconCancel = Control?.FindViewById(Context.Resources.GetIdentifier("android:id/search_close_btn", null, null));
            (iconCancel as ImageView)?.SetColorFilter(Color.White.ToAndroid());
        }
    }
}