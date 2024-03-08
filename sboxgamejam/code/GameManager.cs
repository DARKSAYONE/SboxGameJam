using Sandbox;
using System.Diagnostics;
using System.IO;

public sealed class GameManager : Component
{

	[Property] private List<GameObject> Players = new List<GameObject>();
	[Property] private List<GameObject> SpawnPositions = new List<GameObject>();
	[Property] private List<int> PlayersScore = new List<int>() { 0, 0 };
	
	private bool AllPlayersReady = false;
	private bool PVPRoundActive = false;

	private float timer = 10f;
	private bool timerGo = false;

	protected override void OnStart()
	{
		base.OnStart();
		

	}
	protected override void OnUpdate()
	{
		if(!AllPlayersReady)
		PlayerInitialization();

		if(AllPlayersReady && !PVPRoundActive)
		{	
			PVPRound();
			timerGo = true;
		}

		
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
		}
		Log.Info( Players.Count );		
	}

	
	void PVPRound()
	{
		
	}

	void TimerStart()
	{
		if ( timer > 0 )
		{
			timer -= Time.Delta;
			Log.Info( timer );
		}
		else if(timer < 0)
		{
			timerGo = false;
			timer = 10f;
		}
	}
}
