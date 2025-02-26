using Sandbox;

public sealed class Product : Component
{
	[Property] public int Value { get; set; } = 100;

	protected override void OnStart()
	{
		Log.Info( $"Prodcut Dropped with value: {Value}" );
	}
	protected override void OnAwake()
	{

	}
	protected override void OnUpdate()
	{
	}


}
