using Sandbox;



public sealed class CashCollector : Component
{
	private int totalCash = 0;		//players total cash
	private int unclaimedCash = 0;  //displayed above the cash collector
	private GameObject CashText;
	private TextRenderer worldText;	//Text component for displaying

	protected override void OnStart()
	{
		//create floating WorldText for Unclaimed Cash amount.
		CashText = new GameObject();
		CashText.WorldPosition = GameObject.WorldPosition + Vector3.Up * 50f;

		worldText = CashText.Components.Create<TextRenderer>();
		worldText.Text = $"Cash: ${unclaimedCash}";
		worldText.FontSize = 25f;
	}


	public void UpdateDisplay( int value )
	{
		unclaimedCash = value;

		if ( worldText != null )
		{
			worldText.Text = $" ${unclaimedCash} ";
		}
	}

	public void CollectCash()
	{
		//collects cash and updates UI with total cash amount.
		var processor = Scene.GetAllComponents<Processor>().FirstOrDefault();
		if ( processor != null )
		{
			//call processors collectcash function which stores totalProcessedValue in a variable and then
			//resets totalProcessedValue to 0 and then updates the collector.


			int collected = processor.ProcessorCollectCash();
			totalCash += collected;

			//grabs ui component
			var cashUI = Scene.GetAllComponents<CashUI>().FirstOrDefault();
			if ( cashUI != null )
			{
				cashUI.UpdateCash( totalCash );
			}

			Log.Info( $"Collected ${collected}. Total Cash: ${totalCash}" );
		}


	}


}
