using UnityEngine;
using uLobby;
using uGameDB;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;





public class StoredMessageInfo {
	
	public string message;
	public int key;
	
	public StoredMessageInfo(string _message,int _key)
	{
		message = _message;
		key = _key;
	}
	
}

public class MainMasterServer : uLink.MonoBehaviour {
	
	private Bucket shipBucket = new Bucket("Ships");
	
	string messageOfDestiny;
	
	uLobby.LobbyConnectionError handle;
	
	List<StoredMessageInfo> storedMessages = new List<StoredMessageInfo>();
	
	LobbyPeer debugServer;
	
	// Use this for initialization
	
	Void Awake () {	
		
		Application.targetFrameRate = 60;
	}
	
	
	void Start () {
		
		Application.targetFrameRate = 60;
		
		print (Directory.GetCurrentDirectory() + "\\" + "GameServer.exe");
		Application.runInBackground = true;
	
		
		Lobby.AddListener(this);
		
		ServerRegistry.OnServerAdded += OnServerAdded;
		ServerRegistry.OnServerRemoved += OnServerRemoved;
		
		AccountManager.OnAccountRegistered += OnAccountRegistered;
		
		AccountManager.OnAccountLoggedIn += OnAccountLoggedIn;
		AccountManager.OnAccountLoggedOut += OnAccoutLogedOut;
		
		handle = Lobby.InitializeLobby(30,7050,"ec2-54-229-34-163.eu-west-1.compute.amazonaws.com",8087);
			
		SendDebugInfo("MasterServer init " + handle);
	
	}

	void HandleAccountManagerOnAccountLoggedIn (Account account)
	{
		
	}

	void HandleAccountManagerOnRegisterFailed (string name, AccountError error)
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
			
	}
	
	void OnGUI () {
		
		
		
		GUILayout.Label(messageOfDestiny);	
		
		
	}
	
	private void uLobby_OnLobbyInitialized()
	{
		SendDebugInfo("MasterServer Initialized ");
	}
	
	private void OnServerAdded(ServerInfo server)
	{
		uLink.BitStream dataCopy = server.data;
		
		SendDebugInfo("Server Connected " +  dataCopy.ReadString() + " on " + server.host);
	}
	
	private void OnServerRemoved(ServerInfo server)
	{
		uLink.BitStream dataCopy = server.data;
		
		SendDebugInfo("Server Removed " +  dataCopy.ReadString() + " on " + server.host);
	}
	
	private void SendDebugInfo(string _Message) // MasterServer info
	{
		SendDebugInfo(_Message,0);
	}
	
	private void SendDebugInfo(string _Message,int _key) // Server info
	{
		if(debugServer != null)
		{
			Lobby.RPC("RecievedDebugMessage",debugServer,_Message,_key);
		}
		else
		{
			if(storedMessages.Count < 50)
			{
				
				StoredMessageInfo tmpStorage = new StoredMessageInfo(_Message,_key);
				
				storedMessages.Add(tmpStorage);
				
				print ("Storing message " + _Message + " with key " + tmpStorage.key);
			}
			
			
		}
	}
	
	[RPC]
	public void SetPlayerShip(uLink.BitStream _Ship,LobbyMessageInfo _Info)
	{	
		
		uLink.BitStream _ShipData = _Ship.ReadBitStream();
		
		AccountID tmpAccountId = AccountManager.Master.GetLoggedInAccount(_Info.sender).id;
		
		var setRequest = shipBucket.Set(tmpAccountId.ToString(),_ShipData._data,Encoding.Bitstream);
		
		print ("Updating " + tmpAccountId + " with " + _ShipData._data.Length);
	}

	[RPC]
	public IEnumerator  RequestPlayerShip(LobbyMessageInfo _Info)
	{
		AccountID tmpAccountId = AccountManager.Master.GetLoggedInAccount(_Info.sender).id;
		
		var getRequest = shipBucket.Get(tmpAccountId.ToString());
		
		yield return getRequest.WaitUntilDone();

	    if (getRequest.hasFailed)
	    {
			yield break;
    	}
		
		byte[] byteArray = getRequest.GetValue<byte[]>();
	
		uLink.BitStream tmperStream = new uLink.BitStream(byteArray,true);
		
		print ("Got stream" + tmperStream._data.Length);
		
		Lobby.RPC("ReturnPlayerShip",_Info.sender,tmperStream);
			
	}
	
	[RPC]
	public void RequestLoginInfo(LobbyMessageInfo info)
	{

	}
	

	[RPC]
	public void RemoveServerFromDebug(int _Key)
	{
		Lobby.RPC("RemoveServerEntity",debugServer,_Key);

	}
	
	[RPC]
	public void PassOnDebugInfo(string _Message,int _Key)
	{
		SendDebugInfo(_Message,_Key);
	}
	
	[RPC]
	public void AttemptToLaunchServer()
	{
		SendDebugInfo(Directory.GetCurrentDirectory() + "\\" + "GameServer.exe");
		System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\" + "GameServer.exe","-batchmode");
		
	}
	
	[RPC]
	public void AddDebugServer(LobbyMessageInfo info)
	{
		
		SendDebugInfo("Debug server connected");
		
		
		debugServer = info.sender;
		
		IEnumerable<ServerInfo> tmp = ServerRegistry.GetServers();
	
		foreach(ServerInfo _Info in tmp)
		{
			Lobby.RPC("ConnectedDebugServer",_Info.peer,_Info.peer);
		}
		
		if(storedMessages.Count > 0)
		{
			foreach(StoredMessageInfo message in storedMessages)
			{
				print ("Sending messages with key " + message.key);
				SendDebugInfo(message.message,message.key);
			}
			
			storedMessages.Clear();
		}
		
	}
	
	[RPC]
	public void RemoveDebugServer()
	{
		debugServer = null;
		
	}
	
	
	
	
	[RPC]
	public void AttemptRegister (string _Username, string _Password,LobbyMessageInfo info)
	{
		SendDebugInfo("Attempted Register");
		Request<Account> account =	AccountManager.Master.RegisterAccount(_Username,_Password);
	}
	
	[RPC]
	public void AttemptLogin (string _Username, string _Password,LobbyMessageInfo info)
	{
		SendDebugInfo("Attempted Login");
		Request<Account> account =	AccountManager.Master.LogIn(info.sender,_Username,_Password);
	}

	void OnAccountLoggedIn(Account _Account)
	{
		SendDebugInfo("Account loged in " + _Account.ToString());	
	}
	
	void OnAccoutLogedOut(Account _Account)
	{
		SendDebugInfo("Account loged out " + _Account.ToString());
	}
	
	void OnAccountRegistered(Account _Account)
	{
		SendDebugInfo("Account creation succeded " + _Account.ToString());	
	}
	 
	
}




