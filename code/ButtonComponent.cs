using Sandbox;
//include character, dropper, conveyor, and cash collector?



public class ButtonComponent : Component, Component.ITriggerListener, Component.ICollisionListener
{

	
	[Property] public bool DropperButton { get; set; }  //if true, the button will be used to spawn a Dropper
	[Property] public bool ConveyorButton { get; set; } //if true, the button will be used to spawn a Conveyor
	[Property] public bool ProcessorButton { get; set; } //if true, the button will be used to CollectCash
	[Property] public bool CashCollectorButton { get; set; } //if true, the button will be used to CollectCash





	private ModelRenderer renderer;
	

	


	public void OnTriggerEnter( Collider other )
	{
		Log.Info( $"Button Pressed by {other.GameObject.Name}" );
		renderer.Tint = new Color( 0, 1, 0 );

		//create a new dropper game object on the map nearby.
		if ( DropperButton )
		{
			SpawnDropper();
		}
		if ( ConveyorButton )
		{
			SpawnConveyor();
		}
		if ( ProcessorButton )
		{
			SpawnProcessor();
		}
		if ( CashCollectorButton )
		{
			SpawnCashCollector();
			SpawnCollectButton();
		}

		//destroy button gameobject
		this.GameObject.Destroy();
	}

	public void OnTriggerExit(Collider other)
	{
		
	
		
	}

	protected override void OnAwake()
	{
		renderer = Components.Get<ModelRenderer>();

	}
	protected override void OnUpdate()
	{
		
	}

	//function to spawn a dropper
	private void SpawnDropper()
	{
		//spawn location (subject to change)
		Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Up * 10 + Vector3.Forward * 100;

		//creating the dropper gameobject
		GameObject dropper = new GameObject();
		dropper.WorldPosition = spawnPosition;
		dropper.Name = "Dropper";

		//add modelrenderer component
		var model = dropper.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );

		//add a Box Collider
		var collider = dropper.Components.Create<BoxCollider>();
		//collider.SetSize(new Vector3(20,20,20)); //subject to change.

		//attach Dropper component
		dropper.Components.Create<Dropper>();

		Log.Info( "Dropper Spawned at {spawnPosition}" );
	}


	//function to spawn a converyor
	private void SpawnConveyor()
	{
		//spawn location (subject to change)
		Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Up * 1 + Vector3.Forward * 100;

		//creating the conveyor gameobject
		GameObject conveyor = new GameObject();
		conveyor.WorldPosition = spawnPosition;
		conveyor.WorldScale = new Vector3(1f, 7f, 0.05f);

		conveyor.Name = "Conveyor";

		//add modelrenderer component
		var model = conveyor.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );
		model.Tint = new Color( 186, 88, 64 );

		//add a SolidCollider
		var solidCollider = conveyor.Components.Create<BoxCollider>();
		solidCollider.IsTrigger = false;      //to detect objects.
		solidCollider.Scale = new Vector3(50f,50f,50f); //subject to change.


		//add a triggerCollider
		var triggerCollider = conveyor.Components.Create<BoxCollider>();
		triggerCollider.IsTrigger = true;      //to detect objects.
		triggerCollider.Scale = new Vector3( 50f, 50f, 200f ); //subject to change.
		Log.Info( $"Trigger Collider Created: Position={conveyor.WorldPosition}, Scale={triggerCollider.Scale}" );


		//create rigid body for gravity and interactions with other objects physics.
		var rigidBody = conveyor.Components.Create<Rigidbody>();
		rigidBody.MotionEnabled = false;
		rigidBody.Gravity = false;

		//attach conveyor component
		var ConveyorComponent = conveyor.Components.Create<Conveyor>();
		ConveyorComponent.Speed = 100;
		ConveyorComponent.Direction = Vector3.Left;				//resets the conveyor default direction to this direction.

		Log.Info( "Conveyor Spawned at {spawnPosition}" );
	}


	private void SpawnProcessor()
	{
		//spawn location (subject to change) (right now attempt to spawn at end of conveyor
		Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Up * 25 + Vector3.Forward * 100 + Vector3.Left * 200;

		//create the GameObject
		GameObject processor = new GameObject();
		processor.WorldPosition = spawnPosition;
		processor.WorldScale = new Vector3( 1, 1, 1 );
		processor.Name = "Processor";

		//add modelrenderer
		var model = processor.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );
		model.Tint = new Color( 0, 0, 1 );

		//add a trigger collider
		var triggerCollider = processor.Components.Create<BoxCollider>();
		triggerCollider.IsTrigger = true;
		triggerCollider.Scale = new Vector3( 50f, 50f, 50f );
		//Log.Info( $"Trigger Collider Created: Position = ${processor.WorldPosition}, Scale = ${triggerCollider.Scale}" );

		//attach the ProcessorComponent
		var ProcessorComponent = processor.Components.Create<Processor>();
		Log.Info( "Processor Spawned at " + spawnPosition );


	}



	//function to collect cash
	private void SpawnCashCollector()
	{
		//set spawn
		Vector3 spawnPosition = GameObject.WorldPosition +Vector3.Up * 25 +  Vector3.Forward * 25 + Vector3.Left * 200;

		//Create CashCollector Game Object
		GameObject cashCollector = new GameObject();
		cashCollector.WorldPosition = spawnPosition;
		cashCollector.WorldScale = new Vector3( 1f, 1f, 1f );
		cashCollector.Name = "CashCollector";


		//add model renderer
		var model = cashCollector.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );
		model.Tint = new Color( 1, 0, 1 );

		//add a SolidCollider
		var solidCollider = cashCollector.Components.Create<BoxCollider>();
		solidCollider.IsTrigger = false;      //to detect objects.
		solidCollider.Scale = new Vector3( 50f, 50f, 50f ); //subject to change.


		var cashCollectorComponent = cashCollector.Components.Create<CashCollector>();

		Log.Info( "CashCollector Spawned at " + spawnPosition );
	}

	private void SpawnCollectButton()
	{
		//set spawn
		Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Forward * 25 + Vector3.Left * 150;

		//create gameobject
		GameObject collectButton = new GameObject();
		collectButton.WorldPosition = spawnPosition;
		collectButton.WorldScale = new Vector3( 0.9f, 0.8f, 0.01f );
		collectButton.Name = "CollectCashButton";


		//create model rederer
		var model = collectButton.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dev/box.vmdl" );
		model.Tint = new Color( 0, 1, 0 );  //green

		//trigger collider
		var buttonCollider = collectButton.Components.Create<BoxCollider>();
		buttonCollider.IsTrigger = true;
		buttonCollider.Scale = new Vector3( 50f, 50f, 50f );

		//attach button component
		var buttonComponent = collectButton.Components.Create<CollectCashComponent>(); 

		Log.Info( "Collect Cash Button Spawned at " + spawnPosition );
	}


	//do I create a method to spawn more buttons? Can be used when I want to make more buttons visible in 
	//a new area after I buy all of the things in a previous area.
	private void SpawnButtons()
	{
		Log.Info( $"Dropper Spawned" );
	}

	

}
