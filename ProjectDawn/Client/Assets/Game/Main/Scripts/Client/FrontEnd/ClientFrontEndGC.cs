using UnityEngine;
using uLink;
using uLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ClientFrontEndGC : uLink.MonoBehaviour {
	
	Rect windowRect = new Rect((Screen.width * 0.5f) - 200, (Screen.height * 0.5f) - 80, 400, 160);	
	bool connectedToMS = false;
	
	List<string> redTeam = new List<string>();
	List<string> blueTeam = new List<string>();
	List<string> greenTeam = new List<string>();
	
	public GameObject SplashScreen;
	
	enum AccountState {LOGEDOUT,LOGEDIN};
	AccountState currentState;
	
	enum ReleaseSelection {NONE, SHIPCOMBAT,INTERIOR,PLANET};
	ReleaseSelection currentSelection;
	
	enum LoobyStatus {NONE, STARTED,DECIDED,FINISHED};
	LoobyStatus currentLobbyStatus;
	
	string errorMessage = "";
	
	string username = "";
	string password = "";
	
	public LayerMask lm;
	
	double startTime;
	
	IEnumerable<ServerInfo> servers;
	
	Vector2 scrollPositionModules = Vector2.zero;
	Vector2 scrollPositionItems = Vector2.zero;
	
	int playerCount = 0;
	int serverCount = 0;
	
	float nextUpdate = 0;
	
	ClientMaster CM;
	
	void Awake () {

	}

	// Use this for initialization
	void Start () {
		
		
		CM = GameObject.FindGameObjectWithTag("ClientMaster").GetComponent<ClientMaster>();
	
		AccountManager.OnAccountRegistered += OnAccountRegistered;
		AccountManager.OnRegisterFailed += OnRegisterFailed;
		AccountManager.OnAccountLoggedIn += OnAccountLoggedIn;
		AccountManager.OnAccountLoggedOut += OnAccoutLogedOut;
		AccountManager.OnLogInFailed += OnAccountLoginFail;
		
		if (!CM.IsReturningToHanger())
		{	
			lm = -8;
	
			currentSelection = ReleaseSelection.NONE;
			
			currentState = AccountState.LOGEDOUT;
			
			currentLobbyStatus = LoobyStatus.NONE;
			
			Application.runInBackground = true;
			
			Lobby.AddListener(this);
		
			uLobby.LobbyConnectionError handle = Lobby.ConnectAsClient("ec2-54-229-103-211.eu-west-1.compute.amazonaws.com",7050);
			SendMessage("AddMessage","MasterServer connect state " + handle.ToString());
		}
		else
		{
			lm = -8;
			
			currentLobbyStatus = LoobyStatus.NONE;
			currentState = AccountState.LOGEDIN;
			currentSelection = ReleaseSelection.NONE;
			SplashScreen.SendMessage("StartFade");
			
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(currentState == AccountState.LOGEDIN)
		{
			
			if(nextUpdate < Time.time)
			{
				Lobby.RPC("RequestLoginInfo",LobbyPeer.lobby);
				nextUpdate = Time.time + 3;	
			}
			
			
			if(currentSelection == ReleaseSelection.NONE)
			{
			
			float xPosition = (Input.mousePosition.x - (Screen.width * 0.5f)) / Screen.width;
			float yPosition = (Input.mousePosition.y - (Screen.height * 0.5f)) / Screen.height;
			
			Camera.main.transform.eulerAngles = new Vector3(-(20 * yPosition), 
												20 * xPosition,
												0);
				
				
					
				#region Release Navigation
				if(Input.GetMouseButtonDown(0))
				{
					print ("Raycast!");
					
					RaycastHit hit;
					
					Ray raycast = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
	
					if (Physics.Raycast(Camera.main.transform.position,raycast.direction,out hit,200,lm))
					{
						if(hit.collider.gameObject.name == "ShipHanger")
						{
							servers = ServerRegistry.GetServers();
							currentSelection = ReleaseSelection.SHIPCOMBAT;
							Camera.main.animation.Play();
						}
					}
				}
				#endregion
			}
			
		}
	
	}
	
	void OnGUI () {
		
		if(currentState == AccountState.LOGEDOUT)
		{
			windowRect = GUI.Window(0, windowRect, LoginWindow, "Login Window");	
		}
		else
		{
			if(currentLobbyStatus == LoobyStatus.NONE)
			{
			
				if(currentSelection == ReleaseSelection.SHIPCOMBAT)
				{
					GUILayout.BeginArea(new Rect((Screen.width * 0.5f) - 200,(Screen.height * 0.5f) - 300,400,600));
				
						if(servers.Count() == 0)
						{
							
							GUILayout.Label("NoServers");
						}
						else
						{
							
							foreach(uLobby.ServerInfo info in servers)
							{
								
								if (GUILayout.Button("GameServer"))
								{
									print ("Trying to join server on" + info.host + " on port " + info.port);
									uLink.Network.Connect("ec2-54-229-103-211.eu-west-1.compute.amazonaws.com",info.port);
									//uLink.Network.Connect(info.host,info.port);
								}
							}
						}
						
					GUILayout.EndArea();
					
					if (GUI.Button(new Rect((Screen.width * 0.3f) - 75,5,150,50),"RefreshServers"))
					{
						servers = ServerRegistry.GetServers();
					}
					
					if (GUI.Button(new Rect((Screen.width * 0.5f) - 75,5,150,50),"PLAY"))
					{
						servers = ServerRegistry.GetServers();
						
						if (servers.Count() == 0)
						{	
							SendMessage("AddMessage","No Servers!");		
						}
						else
						{
							
							SendMessage("AddMessage","Connecting to game server");				
						}
					}
					
					if (GUI.Button(new Rect((Screen.width * 0.8f) - 75,5,150,50),"Launch Server"))
					{
						Lobby.RPC("AttemptToLaunchServer",LobbyPeer.lobby);
					}
					
				}
			}
			else
			{
				GUILayout.BeginArea(new Rect((Screen.width * 0.5f) - 200,(Screen.height * 0.5f) - 300,400,600));
				
				GUILayout.BeginHorizontal();
				
				int currentScore = (int)(startTime - uLink.Network.time);
				
				GUILayout.Label("Game starts in " + currentScore);
				
				GUILayout.EndHorizontal();
				
				GUILayout.Label("");			
				
				GUILayout.BeginHorizontal();
				
					GUILayout.BeginVertical();
				
						GUILayout.Label("Red");
					
						foreach(string _Playa in redTeam)
						{
							GUILayout.Label(_Playa);
						}
				
						GUILayout.Label("");
						
						if (GUILayout.Button("Join Red"))
							networkView.RPC("AddToRed",uLink.RPCMode.Server);
				
					GUILayout.EndVertical();
				
					GUILayout.BeginVertical();
				
						GUILayout.Label("Blue");
				
						foreach(string _Playa in blueTeam)
						{
							GUILayout.Label(_Playa);
						}
				
						GUILayout.Label("");
						
						if (GUILayout.Button("Join Blue"))
							networkView.RPC("AddToBlue",uLink.RPCMode.Server);
					
						
					GUILayout.EndVertical();
				
					GUILayout.BeginVertical();
					
						GUILayout.Label("Green");
				
						foreach(string _Playa in greenTeam)
						{
							GUILayout.Label(_Playa);
						}
				
						GUILayout.Label("");
						
						if (GUILayout.Button("Join Green"))
							networkView.RPC("AddToGreen",uLink.RPCMode.Server);
					
					
					GUILayout.EndVertical();
				
				GUILayout.EndHorizontal();
				
				GUILayout.EndArea();
				
				
			}
		}
		
		
		
		
	}
	
	void LoginWindow(int windowID) {
		
		GUILayout.BeginVertical();
		
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Username");
		
				username = GUILayout.TextArea(username);
	
	
			GUILayout.EndHorizontal();
		
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Password");
		
				password = GUILayout.TextArea(password);
	
			GUILayout.EndHorizontal();
		
			GUILayout.BeginHorizontal();
		
				if (GUILayout.Button("Register",GUILayout.Height(60)))
				{
					//Lobby.RPC("AttemptRegister",LobbyPeer.lobby,username,password);
					AccountManager.RegisterAccount(username,password);
				}
				
				if (GUILayout.Button("Login",GUILayout.Height(60)))
				{
					//Lobby.RPC("AttemptLogin",LobbyPeer.lobby,username,password);
					AccountManager.LogIn(username,password);
					print ("blargle");
				}
	
			GUILayout.EndHorizontal();
		
			GUILayout.Label(errorMessage);
		
		GUILayout.EndVertical();
        
    }
	
	[RPC]
	void GetListings(int _Players,int _Servers)
	{
		
		
	}
	
	[RPC]
	void StartGame()
	{
		Application.LoadLevel(1);
		SendMessage("AddMessage","Starting Game");
		
		CM.ReturningToHanger();
	}
	
	[RPC]
	void StartTimer(double _Time)
	{
		startTime = _Time;
	}
	
	[RPC]
	void UpdatePlayerList(int _Team,string _Name,int _LastTeam)
	{
		switch(_LastTeam)
		{
		case -1:
			
			break;
		case 0:
			
			redTeam.Remove(_Name);
			
			break;
		case 1:
			
			blueTeam.Remove(_Name);
			
			break;
		case 2:
			
			greenTeam.Remove(_Name);
			
			break;
		}
		
		switch(_Team)
		{
		case 0:
			
			redTeam.Add(_Name);
			
			break;
		case 1:
			
			blueTeam.Add(_Name);
			
			break;
		case 2:
			
			greenTeam.Add(_Name);
			
			break;
		}
	}
	
	void uLink_OnConnectedToServer()
	{
		SendMessage("AddMessage","Connected to Server");
		networkView.RPC("UserConnected",uLink.RPCMode.Server,username,AccountManager.loggedInAccount.id);
		currentLobbyStatus = LoobyStatus.STARTED;
	}
	
	void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode)
	{
		SendMessage("AddMessage","Server closed");
		
		Camera.main.animation.Play("ReturnFromShipToMain");
		
		currentLobbyStatus = LoobyStatus.NONE;
	
		currentSelection = ReleaseSelection.NONE;
	}
	
	private void uLobby_OnConnected()
	{
		SendMessage("AddMessage","Connected to MasterServer");
		connectedToMS = true;
		
	}
	
	void OnAccountLoggedIn(Account _Account)
	{
		SendMessage("AddMessage","Account loged in " + _Account.ToString());
		
		currentState = AccountState.LOGEDIN;
		
		SplashScreen.SendMessage("StartFade");
		
		SendMessage("SwitchSound");
	}
	
	void OnAccoutLogedOut(Account _Account)
	{
		SendMessage("AddMessage","Account loged out " + _Account.ToString());
	}
	
	void OnAccountLoginFail(string test,uLobby.AccountError _Error)
	{
		errorMessage = _Error.ToString();	
	}
	
	void OnAccountRegistered(Account _Account)
	{
		errorMessage = "Account Succesfuly Registered";		
	}
	
	public void OnRegisterFailed(string _Failure,uLobby.AccountError _Error)
	{
		errorMessage = _Error.ToString();		
	}
	
	
}
