using Sandbox;

public sealed class Fireball : Component
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
		//rb.ApplyForce( Vector3.Forward * speed*Time.Delta ); попытка использовать Rigidbody
		Transform.Position += Transform.Rotation.Forward * speed * Time.Delta;
	}
}
