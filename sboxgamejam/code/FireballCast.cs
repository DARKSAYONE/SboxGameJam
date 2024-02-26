using Sandbox;

public sealed class FireballCast : Component
{

	[Property] public GameObject fireballPrefab;
	[Property] public GameObject fireballspawnPos;

	protected override void OnAwake()
	{
		base.OnAwake();
		
	}
	protected override void OnUpdate()
	{
		if ( !IsProxy )
		{

			if ( Input.Pressed( "Attack1" ) )
			{
				GameObject fireballAttack = fireballPrefab.Clone( fireballspawnPos.Transform.Position );
			}
		}
		else
		{
			Log.Info( "IsProxy ne srabotal" );
		}
	}
}
