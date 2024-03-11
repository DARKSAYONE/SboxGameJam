using Sandbox;
using System.Numerics;
using System.Transactions;

public sealed class EnemyAI : Component
{


	[Property]
	private NavMeshAgent Agent;
	private EnemyNetSpawner ENS;
	[Property] public List<GameObject> Players = new List<GameObject>();

	[Property]
	private float AttackRange = 60.0f;
	[Property]
	private SoundBoxComponent AttackVFX;
	[Property]
	private GameObject TargetPlayer;
	private bool InitPlayers = false;
	private bool TargetPlayerFind = false;
	protected override void OnAwake()
	{
		base.OnAwake();
		Agent = Components.Get<NavMeshAgent>();

		if (Agent == null )
			Log.Error( "EnemyAI:AgentComponent not found" );
		
	}
		

	protected override void OnUpdate()
	{
		

		if ( !InitPlayers )
			InitialPlayers();

		if( InitPlayers && !TargetPlayerFind)
			FindPlayerTarget();

		if ( InitPlayers && TargetPlayerFind )
			MoveToPlayer();
	}

	public void MoveToPlayer()
	{
		//var Player = Scene.GetAllObjects(true).Where(x=>x.Tags.Has("player")).FirstOrDefault();
		if ( TargetPlayer == null )
			Log.Error( "EnemyAI:Player not found" );
		var DistanceToPlayer = Vector3.DistanceBetween( GameObject.Transform.Position, TargetPlayer.Transform.Position );
		//Log.Info(DistanceToPlayer.ToString());
		if(DistanceToPlayer <= AttackRange)
		{
			Agent.Stop();
			//AttackPlayer();
		}
		else
		{
			Agent.MoveTo( TargetPlayer.Transform.Position );
		}
	}

	public void InitialPlayers()
	{
		if ( Players.Count != 2 )
		{
			Players = Scene.GetAllObjects( true ).Where( x => x.Tags.Has( "player" ) && x.Parent.Name == "Scene" ).ToList();
		}
		else
		{
			InitPlayers = true;
			Log.Info( "All players connect" );
		}
		Log.Info( Players.Count );
	}

	public void FindPlayerTarget()
	{
		var DistanceToPlayer1 = Vector3.DistanceBetween( GameObject.Transform.Position, Players[0].Transform.Position );
		var DistanceToPlayer2 = Vector3.DistanceBetween( GameObject.Transform.Position, Players[1].Transform.Position );

		if ( DistanceToPlayer1 > DistanceToPlayer2 )
		{
			TargetPlayer = Players[1];
			TargetPlayerFind = true;
		}
		else if ( DistanceToPlayer1 < DistanceToPlayer2 )
		{
			TargetPlayer = Players[0];
			TargetPlayerFind = true;
		}
	}
	public void AttackPlayer()
	{
		//Attack//

		//AttackVFX.StartSound();
	}
}

	

