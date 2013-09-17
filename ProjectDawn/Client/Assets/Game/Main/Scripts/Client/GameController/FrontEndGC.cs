using UnityEngine;
using uLink;
using uLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class FrontEndGC : GameController {
	
	
	FrontEndUIManager UIManager;
	AccountSession AS;
	
	IEnumerable<ServerInfo> servers;
	
	public Transform cameraPosition1; // Space battlefield
	public Transform cameraPosition2; // Internal conflict
	public Transform cameraPosition3; // Planet assault
	
	public GameObject ServerListingElement;
	public Transform ServerListing;
	
	private Transform LerpToPosition;
	
	private Dictionary<string,GameListing> ServerList = new Dictionary<string, GameListing>();
	
	bool curentlyLerping = false;
	
	enum GameType
	{
		SPACEBATTLEFIELD,
		INTERNALCONFLICT,
		PLANETASSAULT
	}; 
	
	GameType currentGameType;
	
	int pendingGameType;
	
	public void LogedIn()
	{
		UIManager._LoginPanel.ChangeHideState();
		UIManager._TopPanel.ChangeHideState();
		UIManager._BottomPanel.ChangeHideState();
		UIManager.FadeSPlash();
	}
	
	public void SkipLoginPhase()
	{
		UIManager._LoginPanel.SkipPanel();
		UIManager.RemoveSplash();
	}
	
	
	void StartLoginPhase()
	{
		UIManager._LoginPanel.ChangeHideState(); // Show the login panel
		
	}
	
	public void Login (string _Username, string _Password)
	{
		AS.Login(_Username,_Password);
	}
	
	public void GameTypeSelection(int _GameType) // User has selected a game type
	{
		if(_GameType == 0) // Space Battlefield selected
		{
			LerpToPosition = cameraPosition1;
			curentlyLerping = true;
			pendingGameType = 0;
			UIManager._GameModePanel.ChangeHideState();
			
			
		}
		else if (_GameType == 1) // Internal conflict selected
		{
			
			LerpToPosition = cameraPosition2;
			curentlyLerping = true;
			pendingGameType = 1;
		}
		else if (_GameType ==2) // Planet Assault
		{
			
			LerpToPosition = cameraPosition3;
			curentlyLerping = true;
			pendingGameType = 2;
		}
		
		
	}
	
	public void ShowServers()
	{
		UIManager._ServerListingPanel.ChangeHideState();
		
		
		servers = ServerRegistry.GetServers();
		
		ServerList.Clear();
		
		
		
		foreach(ServerInfo _Server in servers)
		{
			/*
			GameObject tmpObj = Instantiate(ServerListingElement) as GameObject;
			tmpObj.transform.parent = ServerListing.transform;
			ServerListing.GetComponent<UIGrid>().Reposition();
			
			
			
			GameListing tmpListing = tmpObj.GetComponent<GameListing>();
			
			*/
	
			uLink.BitStream stream =_Server.data.ReadBitStream();

			short playerCount = stream.ReadInt16();//= _Server.data.Read<string>();
			print (playerCount);
			
			
			short gameState = stream.ReadInt16();
			print (gameState);
			
			
			/*
			 * 
			short gameStatus = _Server.data.Read<short>();
			print(gameStatus);
			
				
			tmpListing.SetServerAttributes(serverName,playerCount,_Server.host,_Server.port,gameStatus);
			
			ServerList.Add(_Server.ToString(),tmpListing);
			
			*/
		}
	}
	
	public void JoinServer (string _ServerIp,int _ServerPort)
	{
		print ("Joining " + _ServerIp);
		AS.JoinGameServer(_ServerIp,_ServerPort);
		
	}
	
	void Awake () {
		
		UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<FrontEndUIManager>();
			
		
	}
	

	// Use this for initialization
	void Start () {
	
		//If we do not currently have an account session lets create one, this will store what state
		//the account is currently in and contain callbacks to ulobby
		
		GameObject actSessionObj = GameObject.FindGameObjectWithTag("AccountSession");
		
		uLobby.ServerRegistry.OnServerDataUpdated += OnServerDataUpdated;
		
		Lobby.AddListener(this);
		
		if(actSessionObj == null)
		{
			GameObject tmpObj = Instantiate(new GameObject()) as GameObject;
			
			tmpObj.tag = "AccountSession";
			tmpObj.name = "AccountSession";
				
			AS = tmpObj.AddComponent<AccountSession>();
			AS._FrontEndGC = this;
			tmpObj.AddComponent<Console>();
			
			StartLoginPhase();
		
		
		}
		else
		{
			//If we already have a session, we must already be loged in!
			AS.GetComponent<AccountSession>()._FrontEndGC = this;	
			SkipLoginPhase();
		}
		
	}

	void OnServerDataUpdated (ServerInfo _Server)
	{
		if(ServerList.Count > 0)
		{
			int testvalue = _Server.data.ReadInt32();
			print(testvalue);
			string serverName = _Server.data.ReadString();
			int playerCount = _Server.data.ReadInt32();
			int gameStatus = _Server.data.ReadInt32();
			
			
			ServerList[_Server.ToString()].SetServerAttributes(serverName,playerCount,_Server.host,_Server.port,gameStatus);
		}
	}
	// Update is called once per frame
	void Update () {
		
		if(curentlyLerping)
		{
		
			float positionalLerp =  0.1f / Vector3.Distance(Camera.main.transform.position,cameraPosition1.position);
			
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,cameraPosition1.position,Time.deltaTime);
			Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,cameraPosition1.rotation,Time.deltaTime);												
			
			if (positionalLerp >= 1)
			{
				Camera.main.transform.position = cameraPosition1.position;
				Camera.main.transform.rotation = cameraPosition1.rotation;											
				curentlyLerping = false;
				
				if(pendingGameType == 0)
				{
					currentGameType = GameType.SPACEBATTLEFIELD;
					
					
				}else if (pendingGameType == 1)
				{
					
					
				}else if (pendingGameType == 2)
				{
					
					
					
				}
		
			
				
			}

		}
	
	}
}
