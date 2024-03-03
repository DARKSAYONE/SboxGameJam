using Sandbox;

public sealed class EnemyNetSpawner : Component, GeneralGame.IUse
{
	[Property]
	public GameObject EnemyPrefab;

	public void OnUse( Guid pickerId )
	{
		
	}

	protected override void OnUpdate()
	{

		if ( !IsProxy ) { 
		if ( Input.Pressed( "use" ) )
		{
			netSpawn();
		}
	}

	}

	void netSpawn()
	{
		var nowSpawn = EnemyPrefab.Clone( GameObject.Transform.World );
		nowSpawn.NetworkSpawn();
	}
}
