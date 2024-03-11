using Sandbox;
using System.Diagnostics;
using System.IO;

public sealed class GameManager : Component
{

	[Property] private List<GameObject> Players = new List<GameObject>();
	[Property] private List<GameObject> SpawnPositions = new List<GameObject>();
	[Property] private List<int> PlayersScore = new List<int>() { 0, 0 };
	[Property] private GameObject BaseSpawnPos;
	
	private bool AllPlayersReady = false;

	private bool RoundOn = false;
	private float RoundStartTimer = 10f;
	private bool timerGo = false;
	private bool DeployPlayers = false;
	protected override void OnStart()
	{
		base.OnStart();
		

	}
	protected override void OnUpdate()
	{
		if(!AllPlayersReady)
		PlayerInitialization();

		if(AllPlayersReady && !RoundOn)
		{
			TimerBeforeRoundStart();
		}

		if(AllPlayersReady && RoundOn)
		{
			PVPRound();
			
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
		}
		Log.Info( Players.Count );		
	}
	
	void PVPRound()
	{
		bool CheckWin = false;
		if (!DeployPlayers)
		{
			for ( int i = 0; i < Players.Count; i++ )
			{
				Players[i].Transform.Position = SpawnPositions[i].Transform.Position;
			}
			Log.Info( "Players tele to spawn positions" );
			DeployPlayers = true;
			CheckWin = true;
		}

		if( CheckWin )
		{
			//Написать шнягу что будет ждать информацию о смерти одного из игроков.
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

			RoundOn = true;
			RoundStartTimer = 10f;
		}
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
