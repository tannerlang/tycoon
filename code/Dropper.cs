using Sandbox;

public sealed class Dropper : Component
{
	[Property] public float DropRate { get; set; } = 4f;
	[Property] public bool DrawDebugLines { get; set; } = false;   
	[Property] public int ProductValue { get; set; } = 100;

	private TimeSince lastDrop = 0;

	protected override void OnAwake()
	{

	}
	protected override void OnUpdate()
	{
		if ( lastDrop >= DropRate )
		{
			spawnProduct();
			lastDrop = 0;
		}

		//Draw Debug for collider
		if ( DrawDebugLines )
		{
			SetDebugLines();

		}
	}

	private void spawnProduct()
	{
		// Define the local position of the nozzle relative to the dropper
		Vector3 localNozzlePosition = new Vector3( -33f, 0f, 35f );

		// Convert local nozzle position to world position using the dropper's transform
		Vector3 spawnPosition = GameObject.WorldTransform.ToWorld(new Transform( localNozzlePosition )).Position;

		GameObject product = new GameObject();
		product.WorldPosition = spawnPosition;
		product.WorldScale = new Vector3( 0.1f, 0.1f, 0.1f );
		product.Name = "Dropped Product";

		//Create the Model
		var model = product.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );

		//Attach collider
		var collider = product.Components.Create<BoxCollider>();

		//Attach product component
		var ProductComponent = product.Components.Create<Product>();
		ProductComponent.Value = ProductValue;

		Log.Info( "Product Dropped at ${spawnPosition}" );		
	}

	public void SetDebugLines()
	{
		foreach ( var collider in Components.GetAll<BoxCollider>() )
		{
			Vector3 center = GameObject.WorldPosition;
			Vector3 halfSize = collider.Scale * 0.5f * GameObject.WorldScale;

			Vector3 mins = center - halfSize;
			Vector3 maxs = center + halfSize;

			Gizmo.Draw.LineBBox( new BBox( mins, maxs ) );
		}
	}

}

