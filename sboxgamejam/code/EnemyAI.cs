using Sandbox;
using System.Numerics;
using System.Transactions;

public sealed class EnemyAI : Component, IStats
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

	[Sync][Property] public int Level { get; set; }
	[Sync][Property] public int Experience { get; set; }
	[Sync][Property] public float HP { get; set; }
	[Sync][Property] public float MaxHP { get; set; }
	[Sync][Property] public int Mana { get; set; }
	[Sync][Property] public int MaxMana { get; set; }
	[Sync][Property] public float ManaRegen { get; set; }
	[Sync][Property] public int PhysicalPower { get; set; }
	[Sync][Property] public int MindPower { get; set; }
	[Sync][Property] public int Protection {get; set; }
	[Sync][Property] public int Fortitude { get; set; }
	[Sync][Property] public float MovementSpeed { get; set; }
	[Sync][Property] public float HitSpeed { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		HP = MaxHP;
		Mana = MaxMana;
	}
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

	public void TakeDamage( Guid attackerGUID )
	{
		PlayerController attackerController = Scene.Directory.FindByGuid( attackerGUID ).Components.Get<PlayerController>();
		//Log.Info( "hi lol" );
		if ( !IsProxy && attackerController.DamageDelay <= 0 )
		{
			Log.Info( "The fireball hit its target!" );
			Log.Info( "Damage delay is set to 2." );
			attackerController.DamageDelay = 2f;
			Log.Info( $"Target HP before reduction: {HP}" );
			var playerDamage = attackerController.MindPower * 5;
			HP = MathF.Max( HP - playerDamage, 0f );
			HP -= playerDamage;
			Log.Info( $"Damage dealt: {playerDamage}" );
			Log.Info( $"Target REAL HP: {HP}" );

			if ( HP <= 0f )
			{
				Log.Info( $"Oh damn it! {GameObject} died! :<" );
				ActivateDeathState(attackerGUID);
			}
		}
	}

	public void ActivateDeathState(Guid attackerGUID)
	{
		PlayerController attackerController = Scene.Directory.FindByGuid( attackerGUID ).Components.Get<PlayerController>();
		attackerController.Experience += (int)MathF.Round(MaxHP * 10);
		Log.Info($"You got {MaxHP * 10} EXP!");

		var expToLevelUp = 1000 * MathF.Pow( 1.2f, attackerController.Level );
		if ( attackerController.Experience >= expToLevelUp)
		{
			attackerController.Experience -= (int)MathF.Round(expToLevelUp);
			attackerController.Level += 1;
			// Add random stat increasement here
		}
		GameObject.Destroy();
	}
}

	

