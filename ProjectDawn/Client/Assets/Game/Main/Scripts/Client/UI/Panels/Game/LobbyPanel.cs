using UnityEngine;
using System.Collections;

public class LobbyPanel : UIPanelController {
	
	// Use this for initialization
	void Start () {
		
		currentlyHidden = false;
		
		hidePosition = new Vector3(0,-1000,0);
		showPosition = new Vector3(0,0,0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
