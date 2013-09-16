using UnityEngine;
using uLink;
using uLobby;
using System.Collections.Generic;
using System.Collections;


public struct NetworkProfile {
	
	public uLink.NetworkPlayer player;
	public string name;
	
}

public class MainServerController : uLink.MonoBehaviour {
	
	
	//Initilised values ----------------------------------------
	
	short minimumPlayers = 2;
	short startGameTime = 2; // In seconds
	short resultsTime = 2; // In seconds
	short roundTime = 2; // In miniutes
	
	//----------------------------------------------------------
	
	enum GameState
	{
		WAITINGFORPLAYERS,
		STARTING,
		PLAYING,
		RESULTS
	} GameState	currentGameState = GameState.WAITINGFORPLAYERS;
	
	int playerCount = 0;
	
	float currentGameTimer = 0; // In seconds
	
	float currentStartingTimer = 0; // in seconds
	
	float currenResultsTimer = 0; // In seconds
	
	bool serverDetailsNeedUpdating = false;
	
	float serverUpdateInterval = 2;
	
	//Revised code above this
	
	public	GameObject creatorShip;
	public	GameObject ownerShip;
	public	GameObject proxyShip;
	
	double timeTillStart;
	
	enum ServerState {NONE,LOBBY,PREGAME,PLAYING,POSTGAME};
		
	ServerState curentServerState;
	
	Dictionary<int,NetworkProfile> unassignedPlayers = new Dictionary<int, NetworkProfile>();
	
	Dictionary<int,NetworkProfile> redTeam = new Dictionary<int, NetworkProfile>();
	
	Dictionary<int,NetworkProfile> blueTeam = new Dictionary<int, NetworkProfile>();
	
	Dictionary<int,NetworkProfile> greenTeam = new Dictionary<int, NetworkProfile>();
	
	void UpdateServerListing () {
		
		
	}
	
	// Use this for initialization
	void Start () {
		
		Application.runInBackground = true;
		
		Lobby.AddListener(this);
	
		uLobby.LobbyConnectionError handle = Lobby.ConnectAsServer("ec2-54-229-103-211.eu-west-1.compute.amazonaws.com",7050);
		SendDebugInfo("MasterServer connect state " + handle.ToString());
	
		playerCount = 3;
	}
	
	void OnDestroy() {
		
		Lobby.RPC("RemoveServerFromDebug",LobbyPeer.lobby,networkView.owner.id);
		
	}
	
	void SwitchState(GameState _SwitchedState)
	{
		if(_SwitchedState == GameState.WAITINGFORPLAYERS) // We must be restarting the game
		{
			
		}
		else if (_SwitchedState == GameState.STARTING) // We must be starting
		{
			currentStartingTimer = (short)startGameTime;
			
		}
		else if (_SwitchedState == GameState.PLAYING) // we must have started
		{
			currentGameTimer = (short)roundTime;
			//currentGameTimer *= 60;
			
		}
		else if (_SwitchedState == GameState.RESULTS) // we must have ended
		{
			currenResultsTimer = (short)resultsTime;
			
		}
		
		currentGameState = _SwitchedState;
		
		print("Im now in " + currentGameState);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(currentGameState == GameState.PLAYING)
		{
			if(currentGameTimer <= 0)
			{
				SwitchState(GameState.RESULTS);
			}
			else
			{
				currentGameTimer -= Time.deltaTime;
			}
		}
		else if (currentGameState == GameState.STARTING)
		{
			if(currentStartingTimer <= 0)
			{
				SwitchState(GameState.PLAYING);
			}
			else
			{
				currentStartingTimer -= Time.deltaTime;
			}
		}
		else if (currentGameState == GameState.RESULTS)
		{
			if(currenResultsTimer <= 0)
			{
				SwitchState(GameState.WAITINGFORPLAYERS);
			}
			else
			{
				currenResultsTimer -= Time.deltaTime;
			}
			
		}
		else if (currentGameState == GameState.WAITINGFORPLAYERS)
		{
			if(playerCount >= minimumPlayers)
			{
				SwitchState(GameState.STARTING);
			}
		}
		
		if(serverDetailsNeedUpdating)
		{
			if(serverUpdateInterval <= 0)
			{
				ServerRegistry.AddServer(7100,name);
				serverUpdateInterval = 2;
			}
			else
			{
				serverUpdateInterval -= Time.deltaTime;
			}
		}
		
		if(curentServerState == ServerState.PREGAME)
		{
			if(timeTillStart < uLink.Network.time)
			{
				
				foreach(KeyValuePair<int,NetworkProfile> val in redTeam)
				{
					SendDebugInfo("Spawning " + val.Value.name + " for red");
					
					Ship_Unpacker shipPacker = new Ship_Unpacker();
					
					shipPacker.idKey = 0;
					shipPacker.hullType = 0;
					
					byte tmpByte = 0;
		
					shipPacker.weaponListing = new byte[3];
					
					shipPacker.weaponListing[0] = 0;
					shipPacker.weaponListing[1] = 1;
					
					GameObject tmp = uLink.Network.Instantiate(val.Value.player,proxyShip,ownerShip,creatorShip,new Vector3(0,0,-250 + Random.Range(-20,20)),Quaternion.identity,5);
					
					//GameObject tmp = uLink.Network.Instantiate(val.Value.player,"ClientUnpacker","ClientUnpacker","ServerUnpacker",new Vector3(0,0,-250),Quaternion.identity,5,shipPacker);
		
					
					tmp.name = "" + val.Value.player.id;
					
					print ("Created Unpacker");
				}
				
				foreach(KeyValuePair<int,NetworkProfile> val in blueTeam)
				{
					SendDebugInfo("Spawning " + val.Value.name + " for blue");
					
					
					GameObject tmp = uLink.Network.Instantiate(val.Value.player,proxyShip,ownerShip,creatorShip,new Vector3(0,0,-250 + Random.Range(-20,20)),Quaternion.identity,5);
					
					//GameObject tmp = uLink.Network.Instantiate(val.Value.player,"ClientUnpacker","ClientUnpacker","ServerUnpacker",new Vector3(0,0,-250),Quaternion.identity,5);
		
					
					
					tmp.name = "" + val.Value.player.id;
				}
				
				foreach(KeyValuePair<int,NetworkProfile> val in greenTeam)
				{
					SendDebugInfo("Spawning " + val.Value.name + " for green");
					
					GameObject tmp = uLink.Network.Instantiate(val.Value.player,proxyShip,ownerShip,creatorShip,new Vector3(0,0,-250 + Random.Range(-20,20)),Quaternion.identity,5);
					
					//GameObject tmp = uLink.Network.Instantiate(val.Value.player,"ClientUnpacker","ClientUnpacker","ServerUnpacker",new Vector3(0,0,-250),Quaternion.identity,5);
		
					
					tmp.name = "" + val.Value.player.id;
					
				}
				
				
				curentServerState = ServerState.PLAYING;
			}
			
		}

	
	}
	
	public void RespawnPlayer(uLink.NetworkPlayer player)
	{
		
		GameObject tmp = uLink.Network.Instantiate(player,"ClientUnpacker","ClientUnpacker","ServerUnpacker",new Vector3(0,0,-250),Quaternion.identity,5);
		tmp.name = "" + player.id;
		
	}
	
	public void SendDebugInfo(string _Message)
	{
		print ("Sent Message");
		Lobby.RPC("PassOnDebugInfo",LobbyPeer.lobby,_Message,networkView.owner.id);
	}
	
	[RPC]
	void HostGame()
	{

		SendDebugInfo("Game Started");
			
		networkView.RPC("StartGame",uLink.RPCMode.Others);
		
		timeTillStart = uLink.Network.time + 5;

		curentServerState = ServerState.PREGAME;
	

	}
	
	[RPC]	
	void UserConnected (string _Name,AccountID _ID,uLink.NetworkMessageInfo _Info)
	{
		NetworkProfile tempProf = new NetworkProfile();

		tempProf.name = _Name;
		
		tempProf.player = _Info.sender;
		
		print (_Name);
		
		unassignedPlayers.Add(_Info.sender.id,tempProf);
		
		SendDebugInfo(_Name + " added to unassigned");
		
	}
	
	[RPC]
	void AddToRed (uLink.NetworkMessageInfo _Info)
	{
		string movedFrom;
		int lastTeam = -1;
		
		if(curentServerState == ServerState.LOBBY)
		{
			NetworkProfile tmpProfile = new NetworkProfile();
			
			
			if(unassignedPlayers.ContainsKey(_Info.sender.id))
			{
				tmpProfile = unassignedPlayers[_Info.sender.id];
				unassignedPlayers.Remove(_Info.sender.id);
				
				movedFrom = "Unassigned";
			}
			else
			{
				if(blueTeam.ContainsKey(_Info.sender.id))
				{
					lastTeam = 1;
					
					tmpProfile = blueTeam[_Info.sender.id];
					blueTeam.Remove(_Info.sender.id);
					
					movedFrom = "Blue";
				}
				else
				{
					if(greenTeam.ContainsKey(_Info.sender.id))
					{
						lastTeam = 2;
						
						tmpProfile = greenTeam[_Info.sender.id];
						greenTeam.Remove(_Info.sender.id);
						
						movedFrom = "Green";
					}
					else
					{
						SendMessage("AddMessage","User is already in red!");
						return;	
					}
				}
			}
			
			SendDebugInfo(tmpProfile.name + " added to red, moved from " + movedFrom);
			
			
			redTeam.Add(_Info.sender.id,tmpProfile);
				
			networkView.RPC("UpdatePlayerList",uLink.RPCMode.All,0,tmpProfile.name,lastTeam);
		}
	}
	
	[RPC]
	void AddToBlue (uLink.NetworkMessageInfo _Info)
	{
		string movedFrom;
		int lastTeam = -1;
		
		if(curentServerState == ServerState.LOBBY)
		{
			NetworkProfile tmpProfile = new NetworkProfile();
			
			
			
			if(unassignedPlayers.ContainsKey(_Info.sender.id))
			{
				print ("was on no team");
				
				tmpProfile = unassignedPlayers[_Info.sender.id];
				unassignedPlayers.Remove(_Info.sender.id);
				
				movedFrom = "Unassigned";
			}
			else
			{
				if(redTeam.ContainsKey(_Info.sender.id))
				{
					lastTeam = 0;
		
					
					tmpProfile = redTeam[_Info.sender.id];
					redTeam.Remove(_Info.sender.id);
					
					movedFrom = "Red";
				}
				else
				{
					
					
					if(greenTeam.ContainsKey(_Info.sender.id))
					{
						lastTeam = 2;
						
						tmpProfile = greenTeam[_Info.sender.id];
						greenTeam.Remove(_Info.sender.id);
						
						movedFrom = "Green";
					}
					else
					{
						SendDebugInfo("User is already in blue!");
						return;	
					}
				}
			}
			
			print (tmpProfile.name);
			
			SendDebugInfo(tmpProfile.name + " added to blue, moved from " + movedFrom);
			
			
			blueTeam.Add(_Info.sender.id,tmpProfile);
				
			networkView.RPC("UpdatePlayerList",uLink.RPCMode.Others,1,tmpProfile.name,lastTeam);
		}
	}
	
	[RPC]
	void AddToGreen (uLink.NetworkMessageInfo _Info)
	{
		string movedFrom;
		int lastTeam = -1;
		
		if(curentServerState == ServerState.LOBBY)
		{
			NetworkProfile tmpProfile = new NetworkProfile();
			
			
			if(unassignedPlayers.ContainsKey(_Info.sender.id))
			{
				tmpProfile = unassignedPlayers[_Info.sender.id];
				unassignedPlayers.Remove(_Info.sender.id);
				
				movedFrom = "Unassigned";
			}
			else
			{
				if(redTeam.ContainsKey(_Info.sender.id))
				{
					lastTeam = 0;
					
					tmpProfile = redTeam[_Info.sender.id];
					redTeam.Remove(_Info.sender.id);
					
					movedFrom = "Red";
				}
				else
				{
					if(blueTeam.ContainsKey(_Info.sender.id))
					{
						lastTeam = 1;
						
						tmpProfile = blueTeam[_Info.sender.id];
						blueTeam.Remove(_Info.sender.id);
						
						movedFrom = "Blue";
					}
					else
					{
						SendMessage("AddMessage","User is already in green!");
			
						return;	
						
					}
				}
			}
			
			SendDebugInfo(tmpProfile.name + " added to green, moved from " + movedFrom);
			
			greenTeam.Add(_Info.sender.id,tmpProfile);
				
			networkView.RPC("UpdatePlayerList",uLink.RPCMode.All,2,tmpProfile.name,lastTeam);
		}
	}
	
	void uLink_OnPlayerConnected(uLink.NetworkPlayer player) {
		
		playerCount++; //Player count goes up by one
		curentServerState = ServerState.LOBBY;
		
		SendDebugInfo("Player Joined" + player.id);
			

	}
	
	void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player) {
		
		playerCount--; //Player count goes down by one
		
		
	}
	
	void uLink_OnConnectedToServer()
	{
		SendDebugInfo("Connected to server");
		
	}
	private void uLobby_OnConnected()
	{
		SendDebugInfo("Connected to MasterServer");
	
		string name = "Main Game Server";
		
		ServerRegistry.AddServer(7100,name);
	}
	
	private void uLobby_OnServerAdded(ServerInfo server)
	{
		SendDebugInfo("Server Connected " + server.data.ToString());
	}
	
}
