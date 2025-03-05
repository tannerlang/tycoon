using Sandbox;

public sealed class ColliderDebugger : Component, Component.ICollisionListener
{
	public void OnCollisionStart( Collision collision )
	{
		Log.Info( $"🛑 Collision detected with: {collision.Other.GameObject.Name}" );
	}
}
