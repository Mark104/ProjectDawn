using UnityEngine;
using System.Collections;

public class SpaceBattlefieldGC : uLink.MonoBehaviour {
	
	float currentTimer;
	
	AccountSession AS;
	
	enum GameState{WAITINGFORPLAYERS,STARTING,RUNNING,RESULTS};
	GameState currentGameState;
	
	enum PlayerState{SETUP,WAITING,INGAME};
	PlayerState currentPlayerState;
	
	void Awake () {
		
		//AS = GameObject.FindGameObjectWithTag("AccountSession").GetComponent<AccountSession>();
		
		currentPlayerState = PlayerState.SETUP;
	}
	
	void Initialize () {
		
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	[RPC]
	void ServerState (short serverState)
	{
		print ("Roar");
		//print (_info.timestamp);
		
		currentGameState = (GameState)serverState;
		
		//currentTimer =	roundTIme + (uLink.Network.time	_info.timestamp);
	}
}
