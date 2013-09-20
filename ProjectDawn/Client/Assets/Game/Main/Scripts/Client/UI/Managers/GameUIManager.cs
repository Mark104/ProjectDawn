using UnityEngine;
using System.Collections;

public class GameUIManager : UIManager {
	
	public LobbyPanel _LobbyPanel;
	public GameUIPanel _GameUIPanel;
	public ResultsPanel _ResultsPanel;
	
	

	// Use this for initialization
	void Start () {
		
		_LobbyPanel = transform.FindChild("Camera/Anchor/LobbyPanel").gameObject.GetComponent<LobbyPanel>();
		_GameUIPanel = transform.FindChild("Camera/Anchor/GameUIPanel").gameObject.GetComponent<GameUIPanel>();
		_ResultsPanel = transform.FindChild("Camera/Anchor/ResultsPanel").gameObject.GetComponent<ResultsPanel>();
	}

	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void SpawnShip()
	{
		
		
	}
	
	public void ShowLobby()
	{
		_ResultsPanel.SkipPanel();
		_LobbyPanel.SkipPanel();
	}
	
	public void ShowResults()
	{
		_ResultsPanel.SkipPanel();
	}
	
	public void HideLobby()
	{
		_LobbyPanel.SkipPanel();
	}
	
	public void LobbyToResults()
	{
		
		_LobbyPanel.SkipPanel();
	}
}
