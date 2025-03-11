using Sandbox;
using System.Collections.Generic;

namespace Sandbox
{

	public sealed class Conveyor : Component, Component.ITriggerListener
	{
		

		
		[Property] public float Speed { get; set; } = 50f;
		[Property] public Vector3 Direction { get; set; } = Vector3.Left;

		//debug property to visualize the collider
		[Property] public bool DrawDebugLines { get; set; } = false;

		//Store objects on the Conveyor
		private HashSet<Rigidbody> objectsOnConveyor = new HashSet<Rigidbody>();
		private HashSet<Rigidbody> transitioningObjects = new HashSet<Rigidbody>();

		//create a dictionary for keeping track of if objects were on another conveyor
		private static Dictionary<Rigidbody, Conveyor> activeConveyors = new();
		

		//Attemp to improve performance with a new method for products
		private class ProductData
		{
			public Vector3 Position;
			public string ProductType;
			public GameObject VisualObject;
			public float TimeRemaining;
		}

		private List<ProductData> fakeProducts = new();
		[Property] public Model FakeProductModel { get; set; }




		protected override void OnUpdate()
		{

			for ( int i = fakeProducts.Count - 1; i >= 0; i-- )
			{
				var product = fakeProducts[i];

				//move fake product along conveyor
				product.VisualObject.WorldPosition = Vector3.Lerp( product.VisualObject.WorldPosition, product.Position, 10f * Time.Delta );
				if ( product.VisualObject != null )
				{
					product.VisualObject.WorldPosition = product.Position;
				}

				//if product reaches the processor remove it from the list
				product.TimeRemaining -= Time.Delta;
				if ( product.TimeRemaining <= 0 )
				{
					ProcessProduct( product );
					product.VisualObject?.Destroy();
					fakeProducts.RemoveAt( i );
				}

			}

			//move Items on conveyor
			foreach ( var rigidBody in objectsOnConveyor )
			{
				//rigidBody.Velocity = Direction.Normal * Speed;
				if ( !transitioningObjects.Contains( rigidBody ) )
				{
					rigidBody.Velocity = Direction.Normal * Speed;
				}
				
			}


			//Draw Debug for collider
			if ( DrawDebugLines )
			{
				SetDebugLines();

			}
		}

		public void AddProduct( string productType, float travelTime )
		{
			GameObject visualProduct = new GameObject();
			visualProduct.WorldPosition = GameObject.WorldPosition + Vector3.Up * 5f;
			var modelRenderer = visualProduct.Components.Create<ModelRenderer>();
			modelRenderer.Model = FakeProductModel;

			fakeProducts.Add( new ProductData
			{
				Position = visualProduct.WorldPosition,
				ProductType = productType,
				VisualObject = visualProduct,
				TimeRemaining = travelTime
			} );
		}

		private void ProcessProduct( ProductData product )
		{
			//grant money, link with processor?
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
			//Log.Info( $"{other.GameObject.Name} entered the conveyor." );

			//get the object on the conveyors PhysicsBody component
			var rigidBody = other.GameObject.GetComponent<Rigidbody>();

			if ( rigidBody != null )
			{
				objectsOnConveyor.Add( rigidBody ); //add object on conveyor to conveyor list.

				//blend direction of product if transitioning from another conveyor
				//check if object was already on another conveyor.
				if ( activeConveyors.TryGetValue( rigidBody, out Conveyor prevConveyor ) && prevConveyor != this )
				{

					// Blend transition between conveyors
					Vector3 transitionDirection = (Direction.Normal + prevConveyor.Direction.Normal).Normal;
					Vector3 targetVelocity = transitionDirection * Speed;

					//gradually change velocity.
					SmoothTransition( rigidBody, prevConveyor.Direction.Normal * prevConveyor.Speed, Direction.Normal * Speed );
				}
				else
				{
					//normal movmement
					rigidBody.Velocity = Direction.Normal*Speed;
				}

				//delay between reference switch
				DelayConveyorSwitch( rigidBody );
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
			//Log.Info( $"{other.GameObject.Name} left the conveyor" );
			
		}

		async void SmoothTransition( Rigidbody rigidBody, Vector3 initVelocity, Vector3 targetVelocity )
		{
			float transitionTime = 10f;
			float elapsedTime = 0f;


			//Log.Info( $"Starting SmoothTransition: {initVelocity} → {targetVelocity} over {transitionTime}s" );
			transitioningObjects.Add(rigidBody);

			while ( elapsedTime < transitionTime )
			{

				await GameTask.Yield();

				float t = elapsedTime / transitionTime; //normalize to 0-1 range

				//interpolate between old direction and new direction

				rigidBody.Velocity = Vector3.Lerp( initVelocity, targetVelocity, t );
				elapsedTime += Time.Delta;  //time since last frame
				//Log.Info( $"Transitioning... t={t}, Velocity={rigidBody.Velocity}" );

			}

			rigidBody.Velocity = targetVelocity;
			transitioningObjects.Remove( rigidBody );
			//Log.Info( $"SmoothTransition complete. Final Velocity: {rigidBody.Velocity}" );
		}


		async void DelayConveyorSwitch( Rigidbody rigidbody )
		{
			await GameTask.Yield();	//waits until next frame
			activeConveyors[rigidbody] = this;
		}

	}
}
