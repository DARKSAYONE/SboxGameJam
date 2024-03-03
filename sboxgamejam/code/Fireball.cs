using Sandbox;

public sealed class Fireball : Component, Component.ITriggerListener
{
	[Property] private Rigidbody rb;
	[Property] public float speed = 100;

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

		if (timeSinceCreation > 5f)
		{
			GameObject.Destroy();
		}
		rb.Velocity = Transform.Rotation.Forward * speed; // попытка использовать Rigidbody
		//Transform.Position += Transform.Rotation.Forward * speed * Time.Delta; // Use when need no move it without gravity
	}

	public void OnTriggerEnter( Collider other )
	{
		if ( other.GameObject.Tags.Has( "player" ) )
		{
			var ownerName = GameObject.Name.Substring( 0, GameObject.Name.LastIndexOf( " - " ));
			Log.Info( ownerName );
			Log.Info( "The fireball hit its target!" );
			other.GameObject.Parent.Components.Get<PlayerStats>().HP -= Scene.GetAllObjects( true ).FirstOrDefault( x => x.Name == ownerName ).Components.Get<PlayerStats>().MindPower * 10;
			Log.Info( Scene.GetAllObjects( true ).FirstOrDefault( x => x.Name == ownerName ).Components.Get<PlayerStats>().MindPower * 10 );
			GameObject.Destroy();
		}
		else
		{
			Log.Info( "fireball hit the obj" );
			GameObject.Destroy();
		}
	}

	public void OnTriggerExit( Collider other )
	{
		
	}
}
