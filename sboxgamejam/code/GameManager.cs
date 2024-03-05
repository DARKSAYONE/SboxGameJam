using Sandbox;
using System.IO;

public sealed class GameManager : Component
{

	[Property] private List<GameObject> Players = new List<GameObject>();
	[Property] private List<GameObject> SpawnPositions = new List<GameObject>();
	
	[Property] private bool AllPlayersReady = false;
	[Property] private bool TestRoundStart = false;
	
	
	protected override void OnStart()
	{
		base.OnStart();

	}
	protected override void OnUpdate()
	{
		if(!AllPlayersReady)
		PlayerInitialization();
		else if(AllPlayersReady && TestRoundStart)
		{
			DeployPlayersToArena();
			TestRoundStart = false;
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
			TestRoundStart = true;
			AllPlayersReady = true;
			Log.Info ( "All players connect" );
		}
		Log.Info( Players.Count );		
	}

	void DeployPlayersToArena()
	{
			for(int i = 0; i < Players.Count; i++)
			{
				Players[i].Transform.Position = SpawnPositions[i].Transform.Position;
			}
		Log.Info( "Players tele to spawn positions" );
	}

}
