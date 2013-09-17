using UnityEngine;
using uLink;
using uLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AccountSession : uLink.MonoBehaviour {
	
	public FrontEndGC _FrontEndGC;

	// Use this for initialization
	void Start () {
		
		DontDestroyOnLoad(this);
		
		
		AccountManager.OnAccountRegistered += OnAccountRegistered;
		AccountManager.OnRegisterFailed += OnRegisterFailed;
		AccountManager.OnAccountLoggedIn += OnAccountLoggedIn;
		AccountManager.OnAccountLoggedOut += OnAccoutLogedOut;
		AccountManager.OnLogInFailed += OnAccountLoginFail;
		
		Application.runInBackground = true;
		Lobby.AddListener(this);
		
		Lobby.ConnectAsClient("ec2-54-229-103-211.eu-west-1.compute.amazonaws.com",7050);
		
		
	}
	
	public void Login (string _Username, string _Password)
	{
		AccountManager.LogIn(_Username,_Password);
	}
	
	
	#region ULobbyCallbacks
	
	private void uLobby_OnConnected()
	{
		SendMessage("AddMessage","Connected to MasterServer");
		
		
	}
	
	void OnAccountLoggedIn(Account _Account)
	{
		print ("Loged in");
		
		_FrontEndGC.LogedIn();
		
		SendMessage("AddMessage","Account loged in " + _Account.ToString());
	}
	
	void OnAccoutLogedOut(Account _Account)
	{
		SendMessage("AddMessage","Account loged out " + _Account.ToString());
	}
	
	void OnAccountLoginFail(string test,uLobby.AccountError _Error)
	{

	}
	
	void OnAccountRegistered(Account _Account)
	{
		
	}
	
	public void JoinGameServer(string _ServerIp,int _ServerPor)
	{
		
		uLink.Network.Connect("192.168.0.183",_ServerPor);
		print ("Connecting to server " + _ServerIp + " with port" + _ServerPor);
		
	}
	
	public void OnRegisterFailed(string _Failure,uLobby.AccountError _Error)
	{
		
	}
	
	#endregion
	
	#region uLinkCallbacks
	
	void uLink_OnConnectedToServer()
	{
		
		SendMessage("AddMessage","Connected to Server");
		
		//networkView.RPC("UserConnected",uLink.RPCMode.Server,username,AccountManager.loggedInAccount.id);
	}
	
	void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode)
	{
		SendMessage("AddMessage","Server closed");
		
		Camera.main.animation.Play("ReturnFromShipToMain");
	}
	
	
	#endregion
	
	
}
