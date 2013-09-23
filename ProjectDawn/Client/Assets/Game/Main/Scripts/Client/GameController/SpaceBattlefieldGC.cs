using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceBattlefieldGC : uLink.MonoBehaviour {
	
	GameUIManager UI;
	
	public GameObject cinematicCamera;
	
	AccountSession AS;
	
	enum GameState{WAITINGFORPLAYERS,STARTING,PLAYING,RESULTS};
	GameState currentGameState;
	
	enum PlayerState{SETUP,WAITING,INGAME};
	PlayerState currentPlayerState;
	
	double currentTimer;
	
	short startingTime;
	short roundTime;
	short resultsTime;
	
	bool oneTimeSetup = false;
	
	void Awake () {
		
		//AS = GameObject.FindGameObjectWithTag("AccountSession").GetComponent<AccountSession>();
		
		currentPlayerState = PlayerState.SETUP;
		
		UI = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameUIManager>();
		
	
	}
	
	void SwitchPlayerState(PlayerState _PlayerState)
	{
		if(_PlayerState == PlayerState.INGAME)
		{
			if(currentPlayerState == PlayerState.SETUP)
			{
				if(currentGameState == GameState.PLAYING) // We pressed spawn and server is playing!
				{
					cinematicCamera.camera.rect = new Rect(0,0,0,0);
					cinematicCamera.SetActive(false);
					UI.HideLobby();
					print ("Spawning Ship");
					//Spawn ship etc
				}	
			}
		}
		else if (_PlayerState == PlayerState.SETUP)
		{
			if(currentGameState == GameState.STARTING) //We've been told to enter setup and game is over so we need to spawn
			{
				cinematicCamera.camera.rect = new Rect(0.34f,0.548f,0.32f,0.34f);
				cinematicCamera.SetActive(true);
				UI.ShowLobby();
				print ("Showing Lobby");
				//Show lobby
			}
		}
		else if (_PlayerState == PlayerState.WAITING)
		{
			if(currentGameState == GameState.RESULTS) // We've been told to enter waiting and server was running, must be game over!
			{
				if(currentPlayerState == PlayerState.SETUP)
				{
					
					
					print("Moving from lobby to results");
					// Game over show results from lobby
					UI.LobbyToResults();
				}
				else
				{
					print ("Showing Results");
					// Game over show results
					
				}
				
			}
		}
		
		currentPlayerState = _PlayerState;
		
	}
	
	void SwitchGameState(GameState _SwitchedState)
	{
		
		
		
		if(_SwitchedState == GameState.WAITINGFORPLAYERS) // We must be restarting the game
		{
			currentGameState = _SwitchedState;
			
		}
		else if (_SwitchedState == GameState.STARTING) // We must be starting
		{
			currentGameState = _SwitchedState;
			
			
			if(currentPlayerState != PlayerState.SETUP)
			{
				SwitchPlayerState(PlayerState.SETUP);
			}
			
		}
		else if (_SwitchedState == GameState.PLAYING) // we must have started
		{
			currentGameState = _SwitchedState;
			
		}
		else if (_SwitchedState == GameState.RESULTS) // we must have ended
		{
			
			cinematicCamera.camera.rect = new Rect(0,0,1,1);
			cinematicCamera.SetActive(true);
			
			currentGameState = _SwitchedState;
			
			UI.ShowResults();
			
			SwitchPlayerState(PlayerState.WAITING);
		}
		
		
		
	}
	
	void ShowResults() {
		
		
		
		
	}
	
	void Start () {
		
			
	}
	
	public void EnterGame ()
	{
		if(currentGameState != GameState.STARTING)
		{
			networkView.RPC("EnterGame",uLink.RPCMode.Server);
			SwitchPlayerState(PlayerState.INGAME);
		}
	}
	
	
	public void Initialize (string _AccountName) {
		
		networkView.RPC("UserConnected",uLink.RPCMode.Server,_AccountName);
		
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
		if(currentTimer < -0)
		{
			currentTimer = 0;
		}
		UI._GameUIPanel.GameTimer.text = "" + (int)currentTimer;
		currentTimer -= Time.deltaTime;
	}
	
	public void JoinTeam (byte _Team)
	{
		switch(_Team)
		{
			case 1:
			
				networkView.RPC("AddToRed",uLink.RPCMode.Server);
				
			break;
			
			case 2:
				
				networkView.RPC("AddToBlue",uLink.RPCMode.Server);
				
			break;
			
			case 3:
				
				networkView.RPC("AddToGreen",uLink.RPCMode.Server);
				
			break;

		}
		
		
		
		
		
	}
	
	
	[RPC]
	void ServerStateChanged (byte _ServerState,uLink.NetworkMessageInfo _Info)
	{
		SwitchGameState((GameState)_ServerState);
		
		print ("Server changed to " + currentGameState);
		
		if(currentGameState == GameState.STARTING)
		{
			UI._GameUIPanel.GameStatus.text = "Till match starts";
			currentTimer = startingTime - (uLink.Network.time - _Info.timestamp);
		}
		else if (currentGameState == GameState.PLAYING)
		{
			UI._GameUIPanel.GameStatus.text = "Till match ends";
			currentTimer = roundTime - (uLink.Network.time - _Info.timestamp);
		}
		else if (currentGameState == GameState.RESULTS)
		{
			UI._GameUIPanel.GameStatus.text = "Till next round";
			currentTimer = resultsTime - (uLink.Network.time - _Info.timestamp);
		}
		
		
	}

	
	[RPC]
	void UpdatePlayerList(byte _CurrentTeam,string _Name,byte _LastTeam)
	{
		print ("Player " + _Name + " moved from " + _LastTeam + " to " + _CurrentTeam);
		UI.UpdatePlayerList(_CurrentTeam,_Name,_LastTeam);
	}
	
	
	[RPC]
	void ServerTeams(int _rubbish,string[] _UnAssignedPlayers,string[] _RedPlayers,string[] _BluePlayers,string[] _GreenPlayers)
	{
		UI.ClearTeams();
		
		
		foreach(string playa in _UnAssignedPlayers)
		{
			UI.SetPlayerToTeam(playa,0);
		}
		foreach(string playa in _RedPlayers)
		{
			UI.SetPlayerToTeam(playa,1);
		}
		foreach(string playa in _BluePlayers)
		{
			UI.SetPlayerToTeam(playa,2);
		}
		foreach(string playa in _GreenPlayers)
		{
			UI.SetPlayerToTeam(playa,3);
		}
		
		UI.OrderPlayerList();
	}
	
	
	[RPC]
	void ServerStatus (short _ServerState,float _CurrentTime,short _StartingTime,short _RoundTime,short _ResultsTime,uLink.NetworkMessageInfo _Info)
	{
		//print (_info.timestamp);
	
		currentTimer = (double)_CurrentTime + (uLink.Network.time - _Info.timestamp);
		
		//print ("Got unassigned players " + _UnassignedPlayers[0]);
	
		
		startingTime = _StartingTime;
		roundTime = _RoundTime;
		resultsTime = _ResultsTime;
		
		UI._LobbyPanel.SetStartingInformation(_StartingTime,_RoundTime,_ResultsTime);
		
		currentGameState = (GameState)_ServerState;
	
	}
}
