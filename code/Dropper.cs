using Sandbox;

public sealed class Dropper : Component
{
	[Property] public float DropRate { get; set; } = 2.0f;


	private TimeSince lastDrop = 0;

	protected override void OnAwake()
	{

	}
	protected override void OnUpdate()
	{
		if ( lastDrop > +DropRate )
		{
			spawnProduct();
			lastDrop = 0;
		}
	}

	private void spawnProduct()
	{
		//Set parameters of the object
		Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Up * 50 + Vector3.Forward * 75;
		GameObject product = new GameObject();
		product.WorldPosition = spawnPosition;
		product.WorldScale = new Vector3( 0.1f, 0.1f, 0.1f );
		product.Name = "Dropped Product";

		//creating the model
		var model = product.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );

		//create collider, necessary to add physics 
		var collider = product.Components.Create<BoxCollider>();

		//create rigid body for gravity and interactions with other objects physics.
		var rigidBody = product.Components.Create<Rigidbody>();
		rigidBody.MotionEnabled = true;
		rigidBody.Gravity = true;

		//attach product component
		product.Components.Create<Product>();

		Log.Info( "Product Dropped at {spawnPosition}" );
	}

}
