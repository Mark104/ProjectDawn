using UnityEngine;
using System.Collections;

public class tempServerJoin : uLink.MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		uLink.Network.Connect("25.150.103.245",6050);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void uLink_OnConnectedToServer()
	{
		print ("Roar");
		
		
	}
	
	
	
}
