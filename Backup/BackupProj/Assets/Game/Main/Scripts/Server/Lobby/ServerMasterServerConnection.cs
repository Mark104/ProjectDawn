using UnityEngine;
using uLink;
using uLobby;
using System.Collections;

public class ServerMasterServerConnection : uLink.MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		Application.runInBackground = true;
		
		Lobby.AddListener(this);
	
		uLobby.LobbyConnectionError handle = Lobby.ConnectAsServer(Settings.ServerIP,7050);
		SendMessage("AddMessage","MasterServer connect state " + handle.ToString());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void uLobby_OnConnected()
	{
		SendMessage("AddMessage","Connected to MasterServer");
	
		string name = "Main Game Server";
		
		ServerRegistry.AddServer(7100,name);
	}
	
	private void uLobby_OnServerAdded(ServerInfo server)
	{
		SendMessage("AddMessage","Server Connected " + server.data.ToString());
	}
}
