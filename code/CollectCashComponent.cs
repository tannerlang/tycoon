using Sandbox;


public sealed class CollectCashComponent : Component, Component.ITriggerListener
{
	public void OnTriggerEnter( Collider other )
	{
		Log.Info( "Cash Collect Button Pressed" );

		var cashCollector = Scene.GetAllComponents<CashCollector>().FirstOrDefault();
		if ( cashCollector != null )
		{
			cashCollector.CollectCash();
		}
	}
}
