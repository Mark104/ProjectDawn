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
	
	public	GameObject creatorShip;
	public	GameObject ownerShip;
	public	GameObject proxyShip;
	
	double gamestartTimer = 5;
	
	double timeTillStart;
	
	enum ServerState {NONE,LOBBY,PREGAME,PLAYING,POSTGAME};
		
	ServerState curentServerState;
	
	Dictionary<int,NetworkProfile> unassignedPlayers = new Dictionary<int, NetworkProfile>();
	
	Dictionary<int,NetworkProfile> redTeam = new Dictionary<int, NetworkProfile>();
	
	Dictionary<int,NetworkProfile> blueTeam = new Dictionary<int, NetworkProfile>();
	
	Dictionary<int,NetworkProfile> greenTeam = new Dictionary<int, NetworkProfile>();
	

	// Use this for initialization
	void Start () {
		
		Application.runInBackground = true;
		
		Lobby.AddListener(this);
	
		uLobby.LobbyConnectionError handle = Lobby.ConnectAsServer(Settings.ServerIP,7050);
		SendDebugInfo("MasterServer connect state " + handle.ToString());
	
		
		curentServerState = ServerState.NONE;
		
		
	
	}
	
	void OnDestroy() {
		
		Lobby.RPC("RemoveServerFromDebug",LobbyPeer.lobby,networkView.owner.id);
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		
		if((gamestartTimer - uLink.Network.time) <= 0 && curentServerState == ServerState.LOBBY)
		{
			SendDebugInfo("Game Started");
			
			networkView.RPC("StartGame",uLink.RPCMode.All);
			
			timeTillStart = uLink.Network.time + 5;
			
			
	
			curentServerState = ServerState.PREGAME;
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
	void UserConnected (string _Name,AccountID _ID,uLink.NetworkMessageInfo _Info)
	{
		NetworkProfile tempProf = new NetworkProfile();

		tempProf.name = _Name;
		
		tempProf.player = _Info.sender;
		
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
			
			SendDebugInfo(tmpProfile.name + " added to blue, moved from " + movedFrom);
			
			
			blueTeam.Add(_Info.sender.id,tmpProfile);
				
			networkView.RPC("UpdatePlayerList",uLink.RPCMode.All,1,tmpProfile.name,lastTeam);
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
		
		if (gamestartTimer == 5)
		{
			curentServerState = ServerState.LOBBY;
			
			gamestartTimer = uLink.Network.time + gamestartTimer;
			
			SendDebugInfo("Player Joined" + player.id);
			
			networkView.RPC("StartTimer",player,gamestartTimer);
		}
		
		
		/*
		GameObject tmp = uLink.Network.Instantiate(player,proxyShip,ownerShip,creatorShip,new Vector3(0,0,-250),Quaternion.identity,5);
		tmp.name = "" + player.id;
		*/
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
