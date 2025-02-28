using Sandbox;
using System.Collections.Generic;

namespace Sandbox
{
	public sealed class Conveyor : Component, Component.ITriggerListener
	{
		//logic will be similar to a button where it detects an Products collider, but then it will move
		//the product along the conveyor.

		//properties for the editor
		//Speed and Direction
		[Property] public float Speed { get; set; } = 100.0f;
		[Property] public Vector3 Direction { get; set; } = Vector3.Left;

		//debug property to visualize the collider
		[Property] public bool DrawDebugLines { get; set; } = false;

		//Store objects on the Conveyor
		private HashSet<Rigidbody> objectsOnConveyor = new HashSet<Rigidbody>();
		protected override void OnStart()
		{
			
		}

		protected override void OnUpdate()
		{

			//move Items on conveyor


			foreach ( var rigidBody in objectsOnConveyor )
			{
				rigidBody.Velocity = Direction.Normal * Speed;
			}


			//Draw Debug for collider
			if ( DrawDebugLines )
			{
				SetDebugLines();

			}
		}

		public void SetDebugLines()
		{
			foreach ( var collider in Components.GetAll<BoxCollider>() )
			{
				Vector3 center = GameObject.WorldPosition;
				Vector3 halfSize = collider.Scale * 0.5f * GameObject.WorldScale;

				Vector3 mins = center - halfSize;
				Vector3 maxs = center + halfSize;

				Gizmo.Draw.LineBBox( new BBox( mins, maxs ) );
			}
		}

		public void OnTriggerEnter( Collider other )
		{
			//Log report that there is a gameObject on the coveyor.
			Log.Info( $"{other.GameObject.Name} entered the conveyor." );

			//get the object on the conveyors PhysicsBody component
			var rigidBody = other.GameObject.GetComponent<Rigidbody>();

			if ( rigidBody != null )
			{
				objectsOnConveyor.Add( rigidBody ); //add object on conveyor to conveyor list.
			}
		}

		public void OnTriggerExit( Collider other )
		{
			var rigidBody = other.GameObject.GetComponent<Rigidbody>();

			if ( rigidBody != null )
			{
				objectsOnConveyor.Remove( rigidBody );	//remove object from the list.
			}

			//Report that Object has left the conveyor.
			Log.Info( $"{other.GameObject.Name} left the conveyor" );
			
		}

	}
}
