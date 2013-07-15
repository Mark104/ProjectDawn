using UnityEngine;
using System.Collections;
using uLink;
using uLobby;
using System.Collections.Generic;

public class DebugServerController : uLink.MonoBehaviour {
	
	Dictionary<int,DebugEntity> DebugEntityListing = new Dictionary<int, DebugEntity>();

	// Use this for initialization
	void Start () {
		
		
		Application.runInBackground = true;
		
		Lobby.AddListener(this);
	
		uLobby.LobbyConnectionError handle = Lobby.ConnectAsServer(Settings.ServerIP,7050);
	
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnGUI() {
		
		
		foreach(KeyValuePair<int,DebugEntity> val in DebugEntityListing)
		{
		
			val.Value.windowRect = GUI.Window(val.Key, val.Value.windowRect,DoEntityInfoWIndow , "Server " + val.Key);
			
			
		}
	}
	
	void DoEntityInfoWIndow(int windowID) {
		
		DebugEntityListing[windowID].scrollPos = GUILayout.BeginScrollView(DebugEntityListing[windowID].scrollPos);
		
			foreach(string msg in DebugEntityListing[windowID].messages)
			{
				
				GUILayout.Label(msg);	
			}
			
			GUI.DragWindow();
		
		
		GUILayout.EndScrollView();
		
    }
	
	private void uLobby_OnConnected()
	{
		Lobby.RPC("AddDebugServer",LobbyPeer.lobby);
	}
	
	void OnDestroy()
	{
		Lobby.RPC("RemoveDebugServer",LobbyPeer.lobby);
		
	}
	
	
	[RPC]
	void	RecievedDebugMessage(string _Message,int _Key)
	{
		print ("Got Message with key " + _Key);
		if(!DebugEntityListing.ContainsKey(_Key))
		{
			DebugEntity tmpEnttiy = new DebugEntity();
			
			tmpEnttiy.AddMessage(_Message);
			
			DebugEntityListing.Add(_Key,tmpEnttiy);
	
		}
		else
		{
			DebugEntityListing[_Key].AddMessage(_Message);

		}
		
		
		
	}
	
	[RPC]
	void	RemoveServerEntity(int _Key)
	{
		DebugEntityListing.Remove(_Key);
	}
}




public class DebugEntity {
	
	public Rect windowRect = new Rect(0,0,300,600);
			
	public Vector2 scrollPos = new Vector2(0,0);
	
	public List<string> messages = new List<string>();
	
	public void AddMessage (string _Message)	
	{
		if(messages.Count > 50)
		{
			messages.RemoveAt(0);
			
		}
			
		messages.Add(_Message);
	}
	
}

