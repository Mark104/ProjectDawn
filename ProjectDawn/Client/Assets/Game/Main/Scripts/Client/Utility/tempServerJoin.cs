using UnityEngine;
using System.Collections;

public class tempServerJoin : uLink.MonoBehaviour {
	
	void Awake () {
		
		uLink.Network.Connect("25.150.103.245",6050);
		
	}
	
	
	// Use this for initialization
	void Start () {
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void uLink_OnConnectedToServer()
	{
		gameObject.GetComponent<SpaceBattlefieldGC>().Initialize("Jimbob");
		
		
	}
	
	
	
}
