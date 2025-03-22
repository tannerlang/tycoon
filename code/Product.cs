using Sandbox;

public sealed class Product : Component
{

	public float MoveSpeed = 50f;
	private Vector3 direction;
	private bool isMoving = false;

	[Property] public int Value { get; set; } = 100;

	protected override void OnStart()
	{
		Log.Info( $"Prodcut Dropped with value: {Value}" );
	}

	protected override void OnUpdate()
	{
		if ( isMoving )
		{
			//Update Position
			WorldPosition += direction * MoveSpeed * Time.Delta;
		}
	}

	//To be called when in Conveyor.OnTriggerEnter();
	public void initializeProduct( Vector3 conveyorDirection )
	{
		direction = conveyorDirection;
		bool isMoving = true;
	}

	protected override void OnAwake()
	{
		
	}

	




}
