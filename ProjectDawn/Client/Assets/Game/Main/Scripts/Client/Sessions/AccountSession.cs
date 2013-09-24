using UnityEngine;
using uLink;
using uLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AccountSession : uLink.MonoBehaviour {
	
	public FrontEndGC _FrontEndGC;
	
	private GameObject loadingScreen;
	
	string serverIp;
	
	int serverPort;
	
	Account myAccount;
	
	public uLink.BitStream shipCode;

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
		
		//Lobby.ConnectAsClient("ec2-54-229-103-211.eu-west-1.compute.amazonaws.com",7050);
		Lobby.ConnectAsClient("192.168.0.3",7050);
		
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
		
		myAccount = _Account;
		
		byte min = 20;
		byte max = 20;
		
		char minchar = (char)min;
		char maxchar = (char)max;
		
		uLink.BitStream _ShipBitStream = new uLink.BitStream(true);
		
		_ShipBitStream.WriteByte(0); //TYPE
		_ShipBitStream.WriteInt16(1); //BASICFIGHTER
		
		_ShipBitStream.WriteByte(1); //HULL
		_ShipBitStream.WriteInt16(1); //HORNET
		
		_ShipBitStream.WriteByte(2); //HARDPOINTS
		_ShipBitStream.WriteInt16(1); //GATTLING GUN
		
		_ShipBitStream.WriteByte(2); //HARDPOINTS
		_ShipBitStream.WriteInt16(1); //GATTLING GUN
		
		_ShipBitStream.WriteByte(3); //MODULES
		_ShipBitStream.WriteInt16(0); //UNGUIDED MISSILE POD
		
		_ShipBitStream.WriteByte(3); //MODULES
		_ShipBitStream.WriteInt16(0); //UNGUIDED MISSILE POD
		
		_ShipBitStream.WriteByte(4); //ENGINE
		_ShipBitStream.WriteInt16(1); //STANDARD ENGINE
		
		Lobby.RPC("SetPlayerShip",LobbyPeer.lobby,_ShipBitStream);
		
		Lobby.RPC("RequestPlayerShip",LobbyPeer.lobby);
		
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
		
		uLink.Network.Connect("25.150.103.245",_ServerPor);
		print ("Connecting to server " + _ServerIp + " with port" + _ServerPor);
		
		serverIp = _ServerIp;
		serverPort = _ServerPor;
		
	}
	
	public void OnRegisterFailed(string _Failure,uLobby.AccountError _Error)
	{
		
	}
	
	#endregion
	
	IEnumerator  LoadLevelInBackground()
	{
		loadingScreen = Instantiate(Resources.Load("LoadingScreen")) as GameObject;
		
		DontDestroyOnLoad(loadingScreen);
		
		yield return Application.LoadLevelAsync(1);
		
		Destroy(loadingScreen);
	}
	
	
	#region uLinkCallbacks
	
	
	[RPC]
	void ReturnPlayerShip(uLink.BitStream _ShipCode)
	{
		uLink.BitStream tempSteam = _ShipCode.ReadBitStream();
		
		print("Loading Ship with " + tempSteam._data.Length);
		
		GameObject tmpObj =	Instantiate(Resources.Load("Unpackers/Unpacker")) as GameObject;
		
		
		tmpObj.GetComponent<BasicFighterUnpacker>().Unpack(tempSteam,2);
	
		/*
		
		bool expectingKey = true;

		for(int i = 0;i < 1000;i++)
		{
			if(_StoredShipCode.bytesRemaining == 0)
			{	
				print ("Read finished");
				break;
			}
			
			if(expectingKey)
			{
				byte key = _StoredShipCode.ReadByte();
				expectingKey = false;
				
				print ("Key is " + key.ToString());
			}
			else
			{
				short val = _StoredShipCode.ReadInt16();
				expectingKey = true;
				
				print ("Value is " + val);
			}
		}
		
		*/
	}
	
	void uLink_OnConnectedToServer()
	{
		
		SendMessage("AddMessage","Connected to Server");
				
		StartCoroutine(LoadLevelInBackground());
	}
	
	void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode)
	{
		SendMessage("AddMessage","Server closed");
		
		Camera.main.animation.Play("ReturnFromShipToMain");
	}
	
	
	#endregion
	
	
}
