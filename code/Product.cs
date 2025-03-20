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
	protected override void OnAwake()
	{
		/*How do i access the conveyor the product is currently on and link its direction with the direction of the convey?*/
		//direction = GetAllconveyorDirection;
		isMoving = true;
	}
	protected override void OnUpdate()
	{
		if ( isMoving )
		{
			//Update Position
			WorldPosition += direction * MoveSpeed * Time.Delta;
		}
	}


}
