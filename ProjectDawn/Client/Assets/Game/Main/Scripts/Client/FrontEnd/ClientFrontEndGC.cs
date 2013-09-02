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
	
	public Transform ShipLocation;
	
	public GameObject SplashScreen;
	
	enum AccountState {LOGEDOUT,LOGEDIN};
	AccountState currentState;
	
	enum ReleaseSelection {NONE, SHIPCOMBAT,INTERIOR,PLANET};
	ReleaseSelection currentSelection;
	
	enum LoobyStatus {NONE, STARTED,DECIDED,FINISHED};
	LoobyStatus currentLobbyStatus;
	
	enum GameSelection {NONE, CUSTOM,QUICKPLAY,EVENT};
	GameSelection currentPlaySelection;

	enum GameSearchStatus {NONE, SEARCHING,LOBBY,PRELOAD};
	GameSearchStatus currentGameSearchStatus;
	
	public Texture2D topFrame;
	
	public Texture2D bottomFrame;
	
	private Rect customGamesRect;
	
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
	
	
	Vector2 scrollValue = new Vector2(0,0);
	
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
			
			currentGameSearchStatus = GameSearchStatus.NONE;
			
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
		
			currentGameSearchStatus = GameSearchStatus.NONE;
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
			
			if(currentSelection == ReleaseSelection.SHIPCOMBAT)
			{
				if(currentPlaySelection == GameSelection.NONE)
				{
					if(Input.GetMouseButton(0))
					{
						
						Camera.main.transform.RotateAround(ShipLocation.position,transform.up,Input.GetAxis("Horizontal") * 0.4f);
					
					}
					
					
				}
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
			if(currentGameSearchStatus == GameSearchStatus.NONE)
			{
				if(currentSelection == ReleaseSelection.SHIPCOMBAT)
				{							
					if(currentPlaySelection == GameSelection.NONE)
					{
						

						GUILayout.BeginArea(new Rect((Screen.width / 2) - 200,80,400,200));
						
							GUILayout.BeginHorizontal();
						
								if (GUILayout.Button ("Custom"))
								{
									currentPlaySelection = GameSelection.CUSTOM;
								}
								if (GUILayout.Button ("Quick"))
								{
									currentPlaySelection = GameSelection.QUICKPLAY;
								}
								if (GUILayout.Button ("Events"))
								{
									currentPlaySelection = GameSelection.EVENT;
								}
						
							GUILayout.EndHorizontal();
						
						GUILayout.EndArea ();	
					}
					else
					{
						if(currentPlaySelection == GameSelection.CUSTOM)
						{
							
							GUI.Window(0, new Rect((Screen.width / 2) - 300,100,600,400), CustomGameWindow, "Custom games listing");
						
						} else if (currentPlaySelection == GameSelection.QUICKPLAY)
						{
							
							
							
						} else if (currentPlaySelection == GameSelection.EVENT)
						{
							
							
							
						}
					}	
				}
			}
			else if (currentGameSearchStatus ==	GameSearchStatus.LOBBY)
			{
				GUI.Window(0, new Rect((Screen.width / 2) - 300,100,600,400), LobbyWindow, "Game Lobby");
				
				/*
				
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
				
				*/
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
	
	void CustomGameWindow(int windowID) {
	
		GUILayout.BeginHorizontal();
		
			if (GUILayout.Button("Refresh Servers"))
			{
				servers = ServerRegistry.GetServers();
			}
			if (GUILayout.Button("Launch Server"))
			{
				Lobby.RPC("AttemptToLaunchServer",LobbyPeer.lobby);
			}
			
		GUILayout.EndHorizontal();
		
			scrollValue = GUILayout.BeginScrollView (scrollValue,GUILayout.Height(400));
	
			if(servers.Count() == 0) // If there are no registered servers we display a label saying nay
			{
				GUILayout.Label("No Servers");
				
			}
			foreach(uLobby.ServerInfo info in servers)
			{
				
				if (GUILayout.Button("GameServer"))
				{
					print ("Trying to join server on" + info.host + " on port " + info.port);
					// "ec2-54-229-103-211.eu-west-1.compute.amazonaws.com"
					//uLink.Network.Connect("ec2-54-229-103-211.eu-west-1.compute.amazonaws.com",info.port);
					uLink.Network.Connect("25.150.103.245",info.port);
				}
			}
		
			GUILayout.EndScrollView();
		
		
    }
	
	void LobbyWindow(int windowID) {
		
		GUILayout.BeginVertical();	
		
		
			GUILayout.Label("Waiting on host to Start");
			
		
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
		
			GUILayout.BeginHorizontal();
			
				GUILayout.Button("Leave Game");
		
				if (GUILayout.Button("Start Game"))
				{
					networkView.RPC("HostGame",uLink.RPCMode.Server);
				}
			
			GUILayout.EndHorizontal();
		
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
		print ("Player" + _Name + " on team " + _Team + " last team was " + _LastTeam);
		
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
	
		currentGameSearchStatus = GameSearchStatus.LOBBY;
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
