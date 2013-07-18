using UnityEngine;

using uLink;
using uLobby;

using System.Collections;

public class MainClientController : uLink.MonoBehaviour {
	
	public void ConnectToServer (string ip, int port)
	{
		uLink.Network.Connect(ip,port);
		SendMessage("AddMessage","Connecting to game server");
		Destroy(Camera.main.gameObject);
		
	}

	// Use this for initialization
	void Start () {
		
		//ConnectToServer("25.150.103.245",7100);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
