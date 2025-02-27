using Sandbox;


public sealed class Processor : Component, Component.ITriggerListener
{
	//Store Total Value of processed products
	private int totalProcessedValue = 0;

	public void OnTriggerEnter( Collider other )
	{

		var product = other.GameObject.GetComponent<Product>();     //get Product component of GameObject that goes through processor.
		if ( product != null )
		{
			totalProcessedValue += product.Value;           //add the products value to the totalProcessedValue.

			//Report to the log
			Log.Info( $"Processed {other.GameObject.Name} for ${product.Value}. Total: ${totalProcessedValue}" );


			//destroy the product
			other.GameObject.Destroy();
		}


	}




}
