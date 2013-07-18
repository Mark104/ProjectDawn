using UnityEngine;
using uLink;
using uLobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ClientMasterServerConnection : uLink.MonoBehaviour {
	
	bool connectedToServer = false;
	IEnumerable<ServerInfo> servers;
	string serverIP;

	// Use this for initialization
	void Start () {
		

		
		Application.runInBackground = true;
		
		Lobby.AddListener(this);
	
		uLobby.LobbyConnectionError handle = Lobby.ConnectAsClient(Settings.ServerIP,7050);
		SendMessage("AddMessage","MasterServer connect state " + handle.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
		if(servers == null && connectedToServer)
		{
			
			servers = ServerRegistry.GetServers();
			
			foreach (ServerInfo server in servers)
			{
				serverIP = server.host;
				SendMessage("AddMessage","Server Accessed " + server.host);
				
			}
		}
	
	}
	
	void OnGUI () {
		
		
		if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 200,200,200),"Enter Server"))
		{
		
			GetComponent<MainClientController>().ConnectToServer(servers.First().host,servers.First().port);
			//GetComponent<MainClientController>().ConnectToServer("82.20.242.150",7100);
		}
		// The servers are looped over three times in order to get a three-pane GUI panel.

		
		
			
		

	}
	
	private void uLobby_OnConnected()
	{
		SendMessage("AddMessage","Connected to MasterServer");
		connectedToServer = true;
		
	}
}
