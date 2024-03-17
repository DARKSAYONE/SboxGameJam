using Sandbox;

public sealed class EnemyNetSpawner : Component, GeneralGame.IUse
{
	[Property] public GameManager GM;
	[Property] private List<GameObject> MobSpawnPos = new List<GameObject>();
	[Property] public GameObject EnemyPrefab;
	[Property] private List<GameObject> Players = new List<GameObject>();
	[Property] private float SpawnDelay = 5.0f;
	[Property] public List<GameObject> Enemies = new List<GameObject>();
	[Property] public GameObject Owner;

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

	public void DelaySpawn()
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

	public void netSpawn()
	{
		
			var nowSpawn = EnemyPrefab.Clone( GameObject.Transform.Position );
			nowSpawn.NetworkSpawn(Owner.Network.OwnerConnection);
			

		Enemies.Add( nowSpawn );
	}

	public void DeleteAllMobs()
	{
		foreach (var m in Enemies )
		{
			m.Destroy();
		}
		Enemies.Clear();
	}
}
