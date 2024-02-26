using Sandbox;

public sealed class FireballCast : Component
{

	[Property] public GameObject fireballPrefab;
	[Property] public GameObject fireballspawnPos;
	[Property] public GameObject cameraFind;
	[Property] private bool findCamera = false; // Prosti ya shas pishy polnyi govnocod

	protected override void OnAwake()
	{
		base.OnAwake();

		//fireballspawnPos = GameObject.Scene.Directory.FindByName( "Camera" );
		

	}
	protected override void OnUpdate()
	{
		

		if ( !IsProxy )
		{
			
			


			if ( Input.Pressed( "Attack1" ) )
			{
				var netFireball = fireballPrefab.Clone( fireballspawnPos.Transform.Position+Vector3.Forward );
				netFireball.NetworkSpawn();

			}
		}
		else
		{
			Log.Info( "IsProxy ne srabotal" );
		}
	}
}
