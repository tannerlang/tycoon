using Sandbox;
//include character, dropper, conveyor, and cash collector?



public class ButtonComponent : Component, Component.ITriggerListener, Component.ICollisionListener
{

	private ModelRenderer renderer;
	//get a reference to the player game object and use with the OnTriggerEnter and Exit functions.

	


	public void OnTriggerEnter( Collider other )
	{
		Log.Info( $"Button Pressed by {other.GameObject.Name}" );
		renderer.Tint = new Color( 0, 1, 0 );

		//create a new dropper game object on the map nearby.
		SpawnDropper();


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
		
	}

	//function to collect cash
	private void CollectCash()
	{

	}


	//do I create a method to spawn more buttons? Can be used when I want to make more buttons visible in 
	//a new area after I buy all of the things in a previous area.
	private void SpawnButtons()
	{
		Log.Info( $"Dropper Spawned" );
	}
	

	
}
