using Sandbox;
using System.ComponentModel.Design;

public sealed class Fireball : Component, Component.ICollisionListener
{
	[Property] public float speed = 100;

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

		if (timeSinceCreation > 10f)
		{
			GameObject.Destroy();
		}
		rb.Velocity = Transform.Rotation.Forward * speed; // попытка использовать Rigidbody
		//Transform.Position += Transform.Rotation.Forward * speed * Time.Delta; // Use when need no move it without gravity
	}

	//[Broadcast]
	public void OnCollisionStart( Collision other )
	{
		if ( IsProxy )
			return;

		if ( other.Other.GameObject.Tags.Has( "player" ))
		{
			var ownerName = GameObject.Name.Substring( 0, GameObject.Name.LastIndexOf( " - " ) );
			Log.Info( ownerName );

			var attackerGUID = Scene.GetAllObjects( true ).FirstOrDefault( x => x.Name == ownerName ).Id;
			other.Other.GameObject.Parent.Components.Get<PlayerController>().TakeDamage( attackerGUID );
		}
		else if ( other.Other.GameObject.Tags.Has( "npc" ) )
		{
			var ownerName = GameObject.Name.Substring( 0, GameObject.Name.LastIndexOf( " - " ) );
			Log.Info( ownerName );

			var attackerGUID = Scene.GetAllObjects( true ).FirstOrDefault( x => x.Name == ownerName ).Id;
			other.Other.GameObject.Parent.Components.Get<EnemyAI>().TakeDamage( attackerGUID );
		}
		else
		{
			Log.Info( "fireball hit the obj" );
		}
		

		


		GameObject.Destroy();
	}

	public void OnCollisionUpdate( Collision other )
	{

	}

	public void OnCollisionStop( CollisionStop other )
	{
		
	}
}
