using Sandbox;
using System.Diagnostics;
using System.IO;

public sealed class GameManager : Component
{

	[Property] public List<GameObject> Players = new List<GameObject>();
	[Property] private List<GameObject> PVPRoundSpawnPos = new List<GameObject>();
	[Property] private List<GameObject> PVERoundSpawnPos = new List<GameObject>();
	[Property] private List<int> PlayersScore = new List<int>() { 0, 0 };
	[Property] private GameObject BaseSpawnPos;
	[Property] private float PVPRoundTime = 30.0f;
	[Property] private float PVERoundTime = 30.0f;
	[Property] public EnemyNetSpawner ENS;
	[Property] public SoundPointComponent Sound;

	private bool AllPlayersReady = false;
	private bool PVPTime = false;
	private bool BeforeRoundTimer = false;
	private float RoundStartTimer = 10f;
	
	private bool timerGo = false;
	private bool DeployPlayersToPVP = false;
	public bool DeployPlayersToPVE = false;
	private bool PVPRoundIsComplete = false;
	private bool PVERoundIsComplete = false;
	protected override void OnStart()
	{
		base.OnStart();
		

	}
	protected override void OnUpdate()
	{
		if(!AllPlayersReady)
		PlayerInitialization();

		if(AllPlayersReady && !BeforeRoundTimer)
		{
			TimerBeforeRoundStart();
		}

		if(AllPlayersReady && BeforeRoundTimer && !PVPRoundIsComplete)
		{
			PVPRound();
		}
		else if(AllPlayersReady && BeforeRoundTimer && PVPRoundIsComplete)
		{
			PVERound();
			
		}

		


		//RoundStopped();
	}

	void PlayerInitialization()
	{
		if (Players.Count != 2 )
		{
			Players = Scene.GetAllObjects( true ).Where( x => x.Tags.Has( "player" ) && x.Parent.Name == "Scene").ToList();
		}
		else
		{
			AllPlayersReady = true;
			Log.Info ( "All players connect" );
			for(int i = 0; i < Players.Count; i++)
			{
				
			}
		}
		Log.Info( Players.Count );		
	}
	
	void PVPRound()
	{
		if (!DeployPlayersToPVP)
		{
			for ( int i = 0; i < Players.Count; i++ )
			{
				Players[i].Transform.Position = PVPRoundSpawnPos[i].Transform.Position;
			}
			Log.Info( "Players tele to spawn positions" );
			DeployPlayersToPVP = true;
			
		}

		if ( DeployPlayersToPVP && !PVPRoundIsComplete)
			PVPRoundTimer();
	}

	void PVERound()
	{
		if(!DeployPlayersToPVE)
		{
			for ( int i = 0; i < Players.Count;i++ )
			{
				Players[i].Transform.Position = PVERoundSpawnPos[i].Transform.Position;
			}
			Log.Info( "Players tele to spawn positions" );
			DeployPlayersToPVE = true;

		}

		if(DeployPlayersToPVE && !PVERoundIsComplete)
		{
			PVERoundTimer();
		}
	}

	void TimerBeforeRoundStart()
	{
		if ( RoundStartTimer > 0 )
		{
			RoundStartTimer -= Time.Delta;
			Log.Info( RoundStartTimer );
		}
		else if(RoundStartTimer < 0)
		{
			BeforeRoundTimer = true;
			RoundStartTimer = 10f;
		}
	}

	void PVPRoundTimer()
	{
		if(PVPRoundTime > 0)
		{
			PVPRoundTime -= Time.Delta;
			Log.Info( PVPRoundTime );
		}
		else if(PVPRoundTime < 0)
		{
			PVPRoundEnd();
			//PVPTime = true;
		}
	}

	void PVERoundTimer()
	{
		if(PVERoundTime > 0)
		{
			PVERoundTime -= Time.Delta;
			Log.Info(PVERoundTime );
		}
		else if(PVERoundTime < 0)
		{
			PVERoundEnd();
		}
	}

	void PVPRoundEnd()
	{
		DeployPlayersToPVP = false;
		BeforeRoundTimer = false;
		PVPRoundIsComplete = true;
		PVPRoundTime = 20.0f;
		PVERoundIsComplete = false;
		Sound.StartSound();
	}

	void PVERoundEnd()
	{
		DeployPlayersToPVE = false;
		BeforeRoundTimer = false;
		PVERoundIsComplete = true;
		PVPRoundIsComplete = false;
		PVERoundTime = 30.0f;
		Sound.StartSound();
		foreach (var Enemy in ENS.Enemies)
		{
			Enemy.Destroy();
		}
		ENS.Enemies.Clear();
	}

	/*[Broadcast]
	void RoundStopped()
	{
		if(Input.Pressed("use"))
		{
			for ( int i = 0; i < Players.Count; i++ )
				Players[i].Transform.Position = BaseSpawnPos.Transform.Position;

			RoundOn = false;
		}

	}
	*/

	/*void DeployPlayersToArena()
	{
		for ( int i = 0; i < Players.Count; i++ )
		{
			Players[i].Transform.Position = SpawnPositions[i].Transform.Position;
		}
		Log.Info( "Players tele to spawn positions" );
	}
	*/
}
