using Sandbox;
using System.Collections.Generic;

namespace Sandbox
{

	public sealed class Conveyor : Component, Component.ITriggerListener
	{

		[Property] public float Speed { get; set; } = 50f;
		[Property] public Vector3 Direction { get; set; } = Vector3.Left;
		[Property] public bool DrawDebugLines { get; set; } = false;     //debug property to visualize the collider

		//Store objects on the Conveyor
		private HashSet<Product> objectsOnConveyor = new HashSet<Product>();
		private HashSet<Product> transitioningObjects = new HashSet<Product>();

		
		protected override void OnUpdate()
		{	
			//Handles Movement.
			foreach ( var product in objectsOnConveyor )
			{
				if ( !transitioningObjects.Contains( product ) )
				{
					product.WorldPosition += Direction.Normal * Speed * Time.Delta;		//Update WorldPosition (Move Product)
				}
			}
			//Conveyor Collider Debug Lines
			if ( DrawDebugLines )
			{
				SetDebugLines();    //Draw Debug for collider
			}
		}


		public void OnTriggerEnter( Collider other )
		{
			//Log.Info( $"{other.GameObject.Name} entered the conveyor." );		//Log report that there is a gameObject on the coveyor.

			//get the object on the conveyors Product Component
			var product = other.GameObject.GetComponent<Product>();
			if ( product != null )
			{
				objectsOnConveyor.Add( product ); //Add the product to the objectsOnConveyor set.
				product.initializeProduct( Direction.Normal );		//initializeProduct
			}
		}

		public void OnTriggerExit( Collider other )
		{
			var product = other.GameObject.GetComponent<Product>();

			if ( product != null )
			{
				objectsOnConveyor.Remove( product );	//Remove product from the set
			}
			//Log.Info( $"{other.GameObject.Name} left the conveyor" );			//Report that Object has left the conveyor.
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
	}
}
