using Sandbox;

public sealed class DefWallSkill : Component
{

	private float creationTime;

	protected override void OnStart()
	{
		base.OnStart();
		creationTime = Time.Now;
	}
	protected override void OnUpdate()
	{
		float timeSinceCreation = Time.Now - creationTime;

		if ( timeSinceCreation > 8f )
		{
			GameObject.Destroy();
		}
	}
}
