using UnityEngine;
using System.Collections;

public class GameListing : MonoBehaviour {
	
	string serverIp;
	int port;
	
	public void SetServerAttributes(string _ServerName,int _PlayerCount,string _Address,int _Port,int status)
	{
		serverIp = _Address;
		port = _Port;
		transform.Find("GameName").gameObject.GetComponent<UILabel>().text = _ServerName;
		transform.Find("PlayerCount").gameObject.GetComponent<UILabel>().text = "" + _PlayerCount;
		
		
		switch(status)
		{
			case 0:
			
				transform.Find("Status").gameObject.GetComponent<UILabel>().text = "Waiting";
			
			break;
			
			case 1:
			
				transform.Find("Status").gameObject.GetComponent<UILabel>().text = "Starting";
			
			break;
			
			case 2:
			
				transform.Find("Status").gameObject.GetComponent<UILabel>().text = "Playing";
			
			break;
			
			case 3:
			
				transform.Find("Status").gameObject.GetComponent<UILabel>().text = "Results";
			
			break;
		}
	
	}

	public void OnClick()
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<FrontEndGC>().JoinServer(serverIp,port);
	}
}
