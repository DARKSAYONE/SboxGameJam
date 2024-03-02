using Sandbox;

public sealed class EnemyAI : Component
{


	[Property]
	private NavMeshAgent Agent;
	/*[Property]
	private GameObject Player;
	*/
	[Property]
	private float AttackRange = 60.0f;

	protected override void OnAwake()
	{
		base.OnAwake();
		Agent = Components.Get<NavMeshAgent>();

		if (Agent == null )
			Log.Error( "EnemyAI:AgentComponent not found" );		
	}

	protected override void OnUpdate()
	{
		MoveToPlayer();
	}

	public void MoveToPlayer()
	{
		var Player = Scene.GetAllObjects(true).Where(x=>x.Tags.Has("player")).FirstOrDefault();
		if ( Player == null )
			Log.Error( "EnemyAI:Player not found" );
		var DistanceToPlayer = Vector3.DistanceBetween( GameObject.Transform.Position, Player.Transform.Position );
		//Log.Info(DistanceToPlayer.ToString());
		if(DistanceToPlayer <= AttackRange)
		{
			Agent.Stop();
		}
		else
		{
			Agent.MoveTo( Player.Transform.Position );
		}
	}
}
