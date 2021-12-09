package crc648b03a8f8c754c4f4;


public class MyRendererP
	extends crc643f46942d9dd1fff9.PickerRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DndApp.Droid.MyRendererP, DndApp.Android", MyRendererP.class, __md_methods);
	}


	public MyRendererP (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == MyRendererP.class)
			mono.android.TypeManager.Activate ("DndApp.Droid.MyRendererP, DndApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public MyRendererP (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == MyRendererP.class)
			mono.android.TypeManager.Activate ("DndApp.Droid.MyRendererP, DndApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public MyRendererP (android.content.Context p0)
	{
		super (p0);
		if (getClass () == MyRendererP.class)
			mono.android.TypeManager.Activate ("DndApp.Droid.MyRendererP, DndApp.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
