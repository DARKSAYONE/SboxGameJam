using Sandbox;

public sealed class EnemyNetSpawner : Component
{
	[Property]
	public GameObject EnemyPrefab;

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
