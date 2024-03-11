using Sandbox;

public sealed class EnemyNetSpawner : Component, GeneralGame.IUse
{
	[Property] public GameManager GM;
	[Property] private List<GameObject> MobSpawnPos = new List<GameObject>();
	[Property] public GameObject EnemyPrefab;
	[Property] private List<GameObject> Players = new List<GameObject>();
	[Property] private float SpawnDelay = 5.0f;
	[Property] public List<GameObject> Enemies = new List<GameObject>();

	public void OnUse( Guid pickerId )
	{
		
	}

	protected override void OnAwake()
	{
		base.OnAwake();
		
	}

	protected override void OnUpdate()
	{
		if ( GM.DeployPlayersToPVE )
			DelaySpawn();
	}

	void DelaySpawn()
	{
		if ( SpawnDelay > 0 )
		{
			SpawnDelay -= Time.Delta;
		}
		else if ( SpawnDelay < 0 )
		{
			SpawnDelay = 5.0f;
			netSpawn();
		}
	}

	void netSpawn()
	{
		for ( int i = 0; i < MobSpawnPos.Count; i++ )
		{
			var nowSpawn = EnemyPrefab.Clone( MobSpawnPos[i].Transform.Position );
			nowSpawn.NetworkSpawn();
			Enemies.Add( nowSpawn );
		}
	}
}
