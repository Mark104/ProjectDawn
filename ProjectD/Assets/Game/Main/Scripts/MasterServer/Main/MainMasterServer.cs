using UnityEngine;
using uLobby;
using System.Collections;

public class MainMasterServer : MonoBehaviour {
	
	
	

	// Use this for initialization
	void Start () {
		
		Application.runInBackground = true;
		
		Lobby.AddListener(this);
		
		ServerRegistry.OnServerAdded += OnServerAdded;
		ServerRegistry.OnServerRemoved += OnServerRemoved;
		
		AccountManager.OnAccountRegistered += OnAccountRegistered;
		
		AccountManager.OnAccountLoggedIn += OnAccountLoggedIn;
		AccountManager.OnAccountLoggedOut += OnAccoutLogedOut;
		
		uLobby.LobbyConnectionError handle = Lobby.InitializeLobby(30,7050,"ec2-54-229-34-163.eu-west-1.compute.amazonaws.com",8087);
			
		SendMessage("AddMessage","MasterServer init " + handle);
	
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
	
	private void uLobby_OnLobbyInitialized()
	{
		
		
		SendMessage("AddMessage","MasterServer Initialized ");
	}
	
	private void OnServerAdded(ServerInfo server)
	{
		uLink.BitStream dataCopy = server.data;
		
		SendMessage("AddMessage","Server Connected " +  dataCopy.ReadString() + " on " + server.host);
	}
	
	private void OnServerRemoved(ServerInfo server)
	{
		uLink.BitStream dataCopy = server.data;
		
		SendMessage("AddMessage","Server Removed " +  dataCopy.ReadString() + " on " + server.host);
	}
	
	
	[RPC]
	public void AttemptRegister (string _Username, string _Password,LobbyMessageInfo info)
	{
		SendMessage("AddMessage","Attempted Register");
		Request<Account> account =	AccountManager.Master.RegisterAccount(_Username,_Password);
	}
	
	[RPC]
	public void AttemptLogin (string _Username, string _Password,LobbyMessageInfo info)
	{
		SendMessage("AddMessage","Attempted Login");
		Request<Account> account =	AccountManager.Master.LogIn(info.sender,_Username,_Password);
	}

	void OnAccountLoggedIn(Account _Account)
	{
		SendMessage("AddMessage","Account loged in " + _Account.ToString());	
	}
	
	void OnAccoutLogedOut(Account _Account)
	{
		SendMessage("AddMessage","Account loged out " + _Account.ToString());
	}
	
	void OnAccountRegistered(Account _Account)
	{
		SendMessage("AddMessage","Account creation succeded " + _Account.ToString());	
	}
	 
	
}




