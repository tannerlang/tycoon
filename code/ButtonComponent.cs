using Sandbox;
//include character, dropper, conveyor, and cash collector?



public class ButtonComponent : Component, Component.ITriggerListener, Component.ICollisionListener
{

	
	[Property] public bool DropperButton { get; set; }  //if true, the button will be used to spawn a Dropper
	[Property] public bool ConveyorButton { get; set; } //if true, the button will be used to spawn a Conveyor
	[Property] public bool ProcessorButton { get; set; } //if true, the button will be used to CollectCash
	[Property] public bool CashCollectorButton { get; set; } //if true, the button will be used to CollectCash





	private ModelRenderer renderer;
	//private Conveyor conveyor;

	


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

		/*spawn location based on the conveyor
		conveyor = Scene.GetAllComponents<Conveyor>().FirstOrDefault();
		//Vector3 spawnPositionFromConveyor = conveyor.WorldPosition + Vector3.Up * 10 + Vector3.Forward * 100;
		*/

		//creating the dropper gameobject
		GameObject dropper = new GameObject();
		dropper.WorldPosition = spawnPosition;
		dropper.WorldScale = new Vector3( 1.5f, 1.5f, 1.5f );
		dropper.WorldRotation = new Rotation( 0f, 0f, 1f, -0.00000004371139f );
		//figure out how to rotate dropper in code...
		dropper.Name = "Dropper";

		//add modelrenderer component
		var model = dropper.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/dropper.vmdl" );
		model.Tint= new Color(42,42,42);

		//should encapsulate all collider building in one function.
		//add a Box Collider
		var collider = dropper.Components.Create<BoxCollider>();
		collider.Scale = new Vector3( 25f,25f,25f );
		

		//add a center box collider
		var centerCollider = dropper.Components.Create<BoxCollider>();
		centerCollider.Scale = new Vector3( 25f, 25f, 50f );
		centerCollider.Center = new Vector3( 0f, 0f, 37.5f );

		//add a top collider
		var topCollider = dropper.Components.Create<BoxCollider>();
		topCollider.Scale = new Vector3( 30f, 15f, 17f );
		topCollider.Center = new Vector3( -27f, 0, 53f );

		//add a nozzle collider
		var nozzleCollider = dropper.Components.Create<BoxCollider>();
		nozzleCollider.Scale = new Vector3( 18f, 15f, 17f );
		nozzleCollider.Center = new Vector3( -33f, 0f, 42.5f );

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
		conveyor.WorldScale = new Vector3(1f,1f,1f);
		conveyor.WorldRotation = new Rotation( 0f, -0f, -0.7071068f, 0.7071068f );

		conveyor.Name = "Conveyor";

		//add modelrenderer component
		var model = conveyor.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/conveyor.vmdl" );
		model.Tint = new Color( 186, 88, 64 );

		//should encapsulate all collider building in one function.
		//add a SolidCollider
		var solidCollider = conveyor.Components.Create<BoxCollider>();
		solidCollider.IsTrigger = false;      //to detect objects.
		solidCollider.Scale = new Vector3( 325f, 60f, 3f ); //subject to change.


		//SolidCollider rails
		var railCollider = conveyor.Components.Create<BoxCollider>();
		railCollider.IsTrigger = false;
		railCollider.Scale = new Vector3( 324f, 5.099964f, 7f );
		railCollider.Center = new Vector3( 0f, 27.5f, 2.5f );

		//solidCollider railsOpen
		var OpenRailCollider = conveyor.Components.Create<BoxCollider>();
		OpenRailCollider.IsTrigger = false;
		OpenRailCollider.Scale = new Vector3( 274.8989f, 5.099964f, 7f );
		OpenRailCollider.Center = new Vector3( -25.10003f, -27.5f, 2.5f );


		//add a triggerCollider
		var triggerCollider = conveyor.Components.Create<BoxCollider>();
		triggerCollider.IsTrigger = true;      //to detect objects.
		triggerCollider.Scale = new Vector3( 335f, 55f, 15f ); //subject to change.
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
		Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Up * 29 + Vector3.Forward * 100 + Vector3.Left * 175;

		//create the GameObject
		GameObject processor = new GameObject();
		processor.WorldPosition = spawnPosition;
		processor.WorldScale = new Vector3( 0.5f, 0.5f, 0.5f );
		processor.Name = "Processor";

		//add modelrenderer
		var model = processor.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/processor.vmdl" );
		//model.Tint = new Color( 0, 0, 1 );

		//should encapsulate all collider building in one function.
		//add a solid colliders
		var solidTopCollider = processor.Components.Create<BoxCollider>();
		solidTopCollider.IsTrigger = false;
		solidTopCollider.Scale = new Vector3( 200, 50f, 50f );
		solidTopCollider.Center = new Vector3( 0f, 0.0000001937151f, 16.50001f );

		//add solid legCollider
		var solidLegCollider = processor.Components.Create<BoxCollider>();
		solidLegCollider.IsTrigger = false;
		solidLegCollider.Scale = new Vector3( 50f, 50f, 50f );
		solidLegCollider.Center = new Vector3( 75f, 0f, -33f );

		//add other solid legCollider
		var otherSolidLegCollider = processor.Components.Create<BoxCollider>();
		otherSolidLegCollider.IsTrigger = false;
		otherSolidLegCollider.Scale = new Vector3( 50f, 50f, 50f );
		otherSolidLegCollider.Center = new Vector3( -75f, 0f, -33f );

		//add a trigger collider
		var triggerCollider = processor.Components.Create<BoxCollider>();
		triggerCollider.IsTrigger = true;
		triggerCollider.Scale = new Vector3( 200f, 31f, 117f );
		triggerCollider.Center = new Vector3( 0f, 17f, 0f );
		//Log.Info( $"Trigger Collider Created: Position = ${processor.WorldPosition}, Scale = ${triggerCollider.Scale}" );

		//attach the ProcessorComponent
		var ProcessorComponent = processor.Components.Create<Processor>();
		Log.Info( "Processor Spawned at " + spawnPosition );


	}



	//function to collect cash
	private void SpawnCashCollector()
	{
		var processor = Scene.GetAllComponents<Processor>().FirstOrDefault();
		//set spawn
		Vector3 spawnPosition = processor.WorldPosition  + Vector3.Down * 20 + Vector3.Backward * 100;

		//Create CashCollector Game Object
		GameObject cashCollector = new GameObject();
		cashCollector.WorldPosition = spawnPosition;
		cashCollector.WorldScale = new Vector3( 1f, 1f, 1f );
		cashCollector.WorldRotation = new Rotation( -0.7071068f, 0.7071068f, -0.000001739259f, 0.000001739259f );
		cashCollector.Name = "CashCollector";


		//add model renderer
		var model = cashCollector.Components.Create<ModelRenderer>();
		model.Model = Model.Load( "models/cashcollector.vmdl" );
		//model.Tint = new Color( 1, 0, 1 );

		//add a SolidCollider
		var solidCollider = cashCollector.Components.Create<BoxCollider>();
		solidCollider.IsTrigger = false;      //to detect objects.
		solidCollider.Scale = new Vector3( 50f, 50f, 50f ); //subject to change.


		var cashCollectorComponent = cashCollector.Components.Create<CashCollector>();

		Log.Info( "CashCollector Spawned at " + spawnPosition );
	}

	private void SpawnCollectButton()
	{
		var cashCollector = Scene.GetAllComponents<CashCollector>().FirstOrDefault();
		//set spawn
		//Vector3 spawnPosition = GameObject.WorldPosition + Vector3.Forward * 25 + Vector3.Left * 150;
		Vector3 spawnPosition = cashCollector.WorldPosition + Vector3.Backward * 50 + Vector3.Down * 9;
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
