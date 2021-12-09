using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.Core.Content;
using DndApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DndApp.Models.MyPicker), typeof(MyRendererP))]
namespace DndApp.Droid
{
    public class MyRendererP : PickerRenderer
	{
		public MyRendererP(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
				Control.SetPadding(16, 0, 40 , 0);
			}
		}

	}
}
