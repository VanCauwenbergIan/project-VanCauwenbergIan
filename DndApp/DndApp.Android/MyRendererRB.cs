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

[assembly: ExportRenderer(typeof(DndApp.Models.MyRadio), typeof(MyRendererRB))]
namespace DndApp.Droid
{
    public class MyRendererRB : RadioButtonRenderer
    {
        public MyRendererRB(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.RadioButton> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ButtonTintList = ColorStateList.ValueOf(Android.Graphics.Color.White);
            }
        }
    }
}