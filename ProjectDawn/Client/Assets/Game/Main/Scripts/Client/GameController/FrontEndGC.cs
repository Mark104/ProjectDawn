using UnityEngine;
using uLink;
using uLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct ServerListingElement {
	
	
	public GameListing _GameListing;
	public GameObject _GameObject;
	
	
}


public class FrontEndGC : GameController {
	
	
	FrontEndUIManager UIManager;
	AccountSession AS;
	
	IEnumerable<ServerInfo> servers;
	
	public Transform cameraPosition1; // Space battlefield
	public Transform cameraPosition2; // Internal conflict
	public Transform cameraPosition3; // Planet assault
	
	public Transform hangerView;
	
	public GameObject ServerListingElement;
	public Transform ServerListing;
	public Transform HangerNode;
	
	private Transform LerpToPosition;
	
	
	private Dictionary<string,ServerListingElement> ServerList = new Dictionary<string, ServerListingElement>();
	
	bool curentlyLerping = false;
	
	enum GameType
	{
		SPACEBATTLEFIELD,
		INTERNALCONFLICT,
		PLANETASSAULT
	}; 
	
	GameType currentGameType;
	
	enum ReadyStatus
	{
		READY,
		EQUIPING,
		WAITING
		
	}
	
	ReadyStatus currentReadyStatus;
	
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
	
	public void EnterHangerMode (bool _IsHangerMode)
	{
		
		//UIManager._BottomPanel.ChangeHideState();
		UIManager._TopPanel.ChangeHideState();
		UIManager._GameModePanel.ChangeHideState();
		
		
		if(_IsHangerMode)
		{
			
			currentReadyStatus = ReadyStatus.EQUIPING;
			curentlyLerping = false;
			Camera.main.transform.position = hangerView.position;
			Camera.main.transform.rotation = hangerView.rotation;
			Camera.main.transform.parent = HangerNode;
			
		}
		else
		{
			currentReadyStatus = ReadyStatus.READY;
			Camera.main.transform.position = cameraPosition1.position;
			Camera.main.transform.rotation = cameraPosition1.rotation;
			Camera.main.transform.parent = null;
		}
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
			UIManager._ShowHangerPanel.ChangeHideState();
			
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
		
		RefreshServers();
		
	}
	
	public void RefreshServers()
	{
		servers = ServerRegistry.GetServers();
		
		foreach(KeyValuePair<string,ServerListingElement> key in ServerList)
		{
			Destroy(key.Value._GameObject);
		}
		
		ServerList.Clear();
		
		
		
		foreach(ServerInfo _Server in servers)
		{
			ServerListingElement tmpObject = new ServerListingElement();
			tmpObject._GameObject = Instantiate(ServerListingElement) as GameObject;
			tmpObject._GameObject.transform.parent = ServerListing.transform;
			ServerListing.GetComponent<UIGrid>().Reposition();
		
			tmpObject._GameListing = tmpObject._GameObject.GetComponent<GameListing>();
			
	
			uLink.BitStream stream =_Server.data.ReadBitStream();
			
			string serverName = stream.ReadString(); // Read server name

			short playerCount = stream.ReadInt16(); // Read the current player count

			
			short gameState = stream.ReadInt16(); // Read the current game state
		
				
			tmpObject._GameListing.SetServerAttributes(serverName,playerCount,_Server.host,_Server.port,gameState);
			
			ServerList.Add(_Server.ToString(),tmpObject);
			
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
	
	void OnDestroy() {
		
		Lobby.RemoveListener(this);
		uLobby.ServerRegistry.OnServerDataUpdated -= OnServerDataUpdated;
		
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
			
			uLink.NetworkViewID tmpId = new uLink.NetworkViewID(0);
			
			tmpObj.AddComponent<uLink.NetworkView>().SetManualViewID(1);
			
			currentReadyStatus = ReadyStatus.READY;
			
			StartLoginPhase();
		
		}
		else
		{
			//If we already have a session, we must already be loged in!
			AS.GetComponent<AccountSession>()._FrontEndGC = this;
			
			currentReadyStatus = ReadyStatus.READY;
			
			SkipLoginPhase();
		}
		
	}

	void OnServerDataUpdated (ServerInfo _Server)
	{
		if(ServerList.Count > 0)
		{
			uLink.BitStream stream =_Server.data.ReadBitStream();
			
			string serverName = stream.ReadString(); // Read server name

			short playerCount = stream.ReadInt16(); // Read the current player count
	
			short gameState = stream.ReadInt16(); // Read the current game state
			
			ServerList[_Server.ToString()]._GameListing.SetServerAttributes(serverName,playerCount,_Server.host,_Server.port,gameState);
		}
	}
	// Update is called once per frame
	void Update () {
		
		if(currentReadyStatus == ReadyStatus.EQUIPING)
		{
			if(Input.GetMouseButton(0))
			{
				
				HangerNode.Rotate(0,(Input.GetAxis("Horizontal") * 10) * Time.deltaTime,0);	
				
			}
			
		}
		
		if(curentlyLerping)
		{
		
			float positionalLerp =  0.1f / Vector3.Distance(Camera.main.transform.position,cameraPosition1.position);
			
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,cameraPosition1.position,Time.deltaTime * 4);
			Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,cameraPosition1.rotation,Time.deltaTime * 4);												
			
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
