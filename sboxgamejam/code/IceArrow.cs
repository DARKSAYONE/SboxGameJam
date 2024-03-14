using Sandbox;

public sealed class IceArrow : Component, Component.ICollisionListener
{

	[Property] public float speed = 300;

	private Rigidbody rb;
	private float creationTime;


	protected override void OnStart()
	{
		base.OnStart();
		creationTime = Time.Now;
		rb = Components.Get<Rigidbody>();
	}


	protected override void OnUpdate()
	{
		float timeSinceCreation = Time.Now - creationTime;

		if ( timeSinceCreation > 5f )
		{
			GameObject.Destroy();
		}
		rb.Velocity = Transform.Rotation.Forward * speed;
	}



	public void OnCollisionStart( Collision other )
	{
		if ( IsProxy )
			return;

		if ( other.Other.GameObject.Tags.Has( "player" ) )
		{
			var ownerName = GameObject.Name.Substring( 0, GameObject.Name.LastIndexOf( " - " ) );
			Log.Info( ownerName );

			var attackerGUID = Scene.GetAllObjects( true ).FirstOrDefault( x => x.Name == ownerName ).Id;
			other.Other.GameObject.Parent.Components.Get<PlayerController>().TakeDamage( attackerGUID );
			other.Other.GameObject.Parent.Components.Get<PlayerController>().Debuff( attackerGUID );
		}
		else
		{
			Log.Info( "IceArrow hit the obj" );
		}

		GameObject.Destroy();
	}
}
