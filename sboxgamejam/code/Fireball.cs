using Sandbox;

public sealed class Fireball : Component
{
	[Property] private Rigidbody rb;
	[Property] public float speed = 100;
	




	protected override void OnStart()
	{
		base.OnStart();
		
		
		rb = Components.Get<Rigidbody>();
	}



	

	protected override void OnUpdate()
	{

		//rb.ApplyForce( Vector3.Forward * speed*Time.Delta ); попытка использовать Rigidbody

		Transform.Position += Vector3.Forward * speed * Time.Delta;
		
		
	}
}

