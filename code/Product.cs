using Sandbox;
using System;

public sealed class Product : Component
{
	[Property] public int Value { get; set; } = 100;		//Dollar Value of the product.
	public float MoveSpeed = 50f;
	private Vector3 direction;

	private bool isFalling = true;
	private float fallDuration = 0.5f;
	private float fallTimer = 0f;
	private float startZ;
	private float targetZ;
	private bool isMoving = false;



	protected override void OnStart()
	{
		Log.Info( $"Prodcut Dropped with value: {Value}" );
	}

	protected override void OnUpdate()
	{
		if ( isFalling )	
		{
			//Smooth Fall (Lerp)
			fallTimer += Time.Delta;
			float t = MathF.Max( 0f, MathF.Min( 1f, fallTimer / fallDuration ) );
			float easedT = 1 - MathF.Pow(1-t, 2);

			var pos = WorldPosition;
			pos.z = startZ.LerpTo(targetZ, easedT);
			WorldPosition = pos;

			if ( t >= 1f )
			{
				isFalling = false;
				isMoving = true;
			}
		}
		else if ( isMoving )
		{
			//Update Position
			WorldPosition += direction * MoveSpeed * Time.Delta;
		}
	}

	//To be called when in Conveyor.OnTriggerEnter();
	public void InitializeProduct( Vector3 conveyorDirection, float conveyorZ )
	{
		direction = conveyorDirection;
		startZ = WorldPosition.z;
		targetZ = conveyorZ + 3.445f;		//3.445 is the visual offset for the product to be sitting on top of the conveyor.
		isFalling = true;
		isMoving = false;
		fallTimer = 0f;
	}

	protected override void OnAwake()
	{
		
	}
}
