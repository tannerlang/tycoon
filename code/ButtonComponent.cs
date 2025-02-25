using Sandbox;
//include character, dropper, conveyor, and cash collector?



public class ButtonComponent : Component, Component.ITriggerListener, Component.ICollisionListener
{

	private ModelRenderer renderer;
	

	public void OnTriggerEnter(Collider other)
	{
		Log.Info( $"Button Pressed by {other.GameObject.Name}" );
		renderer.Tint = new Color (0,1,0);
	}

	public void OnTriggerExit()
	{
		
		//destroy button gameobject
		//GameObject.Destroy();
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

	}
	


}
