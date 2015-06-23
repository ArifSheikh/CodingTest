package myweatherapplication.ui;


public class DailyFragmentAdapter_ViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MyWeatherApplication.UI.DailyFragmentAdapter/ViewHolder, MyWeatherApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DailyFragmentAdapter_ViewHolder.class, __md_methods);
	}


	public DailyFragmentAdapter_ViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DailyFragmentAdapter_ViewHolder.class)
			mono.android.TypeManager.Activate ("MyWeatherApplication.UI.DailyFragmentAdapter/ViewHolder, MyWeatherApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
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
