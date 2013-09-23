using UnityEngine;
using System.Collections;

public class LobbyPanel : UIPanelController {
	
	public UILabel lobbyFont;
	public UILabel roundFont;
	public UILabel resultsFont;
	
	
	public void SetStartingInformation(short lobbyTime,short roundTime,short resultsTime)
	{
		lobbyFont.text = "Lobby Time : " + lobbyTime.ToString();
		roundFont.text = "Round Time : " + roundTime.ToString();
		resultsFont.text = "Results Time : " + resultsTime.ToString();
		
	}
	
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
