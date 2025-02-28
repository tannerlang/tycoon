using Sandbox;


public sealed class Processor : Component, Component.ITriggerListener
{
	//Store Total Value of processed products
	private int totalProcessedValue = 0;

	//reference CashCollector
	private CashCollector cashCollector;


	protected override void OnStart()
	{
		cashCollector = Scene.GetAllComponents<CashCollector>().FirstOrDefault();			//gets the cashcollect component in the scene
	}

	public void OnTriggerEnter( Collider other )
	{

		var product = other.GameObject.GetComponent<Product>();     //get Product component of GameObject that goes through processor.
		if ( product != null )
		{
			totalProcessedValue += product.Value;           //add the products value to the totalProcessedValue.

			//Report to the log
			Log.Info( $"Processed {other.GameObject.Name} for ${product.Value}. Cash Collector Total : ${totalProcessedValue}" );


			//update cashcollect display
			if ( cashCollector != null )
			{
				cashCollector.UpdateDisplay(totalProcessedValue);
			}


			//destroy the product
			other.GameObject.Destroy();
		}


	}


	public int ProcessorCollectCash()		//linked in CashCollector.cs with CashCollect() this returns an int and is called and assigned to a var in CashCollector.CollectCash()
	{
		int collected = totalProcessedValue;
		totalProcessedValue = 0;

		if ( cashCollector != null )
		{
			cashCollector.UpdateDisplay( totalProcessedValue );		//this resets the cashcollectors display to 0, since we collected the cash.
		}

		return collected;
	}




}
