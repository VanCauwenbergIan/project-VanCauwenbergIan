using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using DndApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DndApp.Models.MyEntry), typeof(MyRendererE))]
namespace DndApp.Droid
{
    public class MyRendererE : EntryRenderer
    {

        public MyRendererE(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                Control.SetPadding(16, 8, 16, 0);
            }
        }
    }
}