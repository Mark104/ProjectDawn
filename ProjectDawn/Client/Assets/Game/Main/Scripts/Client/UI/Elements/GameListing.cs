using UnityEngine;
using System.Collections;

public class GameListing : MonoBehaviour {
	
	string serverIp;
	int port;
	
	void SetServerAttributes(string _ServerName,int _PlayerCount,string _Address,int _Port)
	{
		serverIp = _Address;
		port = _Port;
		transform.Find("GameName").gameObject.GetComponent<UILabel>().text = _ServerName;
		transform.Find("PlayerCount").gameObject.GetComponent<UILabel>().text = "" + _PlayerCount;
	}

	public void OnClick()
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<FrontEndGC>().JoinServer(serverIp,port);
	}
}
