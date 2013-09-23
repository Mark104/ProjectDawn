using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager : UIManager {
	
	public LobbyPanel _LobbyPanel;
	public GameUIPanel _GameUIPanel;
	public ResultsPanel _ResultsPanel;
	
	public GameObject UnAssignedListing;
	public GameObject RedListing;
	public GameObject BlueListing;
	public GameObject GreenListing;
	
	public GameObject PlayerNameElement;
	
	
	Dictionary<string,GameObject> UnAssignedList = new Dictionary<string, GameObject>();
	Dictionary<string,GameObject> RedList = new Dictionary<string, GameObject>();
	Dictionary<string,GameObject> BlueList = new Dictionary<string, GameObject>();
	Dictionary<string,GameObject> GreenList = new Dictionary<string, GameObject>();

		
	

	// Use this for initialization
	void Start () {
		
		_LobbyPanel = transform.FindChild("Camera/Anchor/LobbyPanel").gameObject.GetComponent<LobbyPanel>();
		_GameUIPanel = transform.FindChild("Camera/Anchor/GameUIPanel").gameObject.GetComponent<GameUIPanel>();
		_ResultsPanel = transform.FindChild("Camera/Anchor/ResultsPanel").gameObject.GetComponent<ResultsPanel>();
	}

	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ClearTeams()
	{
		foreach(KeyValuePair<string,GameObject> keys in UnAssignedList)
		{
			Destroy(keys.Value);
			
		}
		foreach(KeyValuePair<string,GameObject> keys in RedList)
		{
			Destroy(keys.Value);
			
		}
		foreach(KeyValuePair<string,GameObject> keys in BlueList)
		{
			Destroy(keys.Value);
			
		}
		foreach(KeyValuePair<string,GameObject> keys in GreenList)
		{
			Destroy(keys.Value);
			
		}
		
		
		UnAssignedList.Clear();
		RedList.Clear();
		BlueList.Clear();
		GreenList.Clear();
		
	}
	
	public void UpdatePlayerList(byte _CurrentTeam,string _Name,byte _LastTeam)
	{
		GameObject tmpGORef= null;
		
		print ("Got update for " + _Name);
		
		switch(_LastTeam) // first lets remove him from ze old listing
		{
		case 0:
			
			tmpGORef = UnAssignedList[_Name];
			
			UnAssignedList.Remove(_Name);
			
			break;
			
		case 1:
			
			tmpGORef = RedList[_Name];
			
			RedList.Remove(_Name);
			
			print ("Removed red entry");

			break;
			
		case 2:
			
			tmpGORef = BlueList[_Name];
			
			BlueList.Remove(_Name);
			
			print ("Removed blue entry");
			
			break;
			
			
		case 3:
			
			tmpGORef = GreenList[_Name];
			
			GreenList.Remove(_Name);
			
			print ("Removed green entry");
			
			break;
		}
		
		switch(_CurrentTeam) // first lets remove him from ze old listing
		{
		case 1:
			
			tmpGORef.transform.parent = RedListing.transform;
			RedList.Add(_Name,tmpGORef);
			
			print ("added to red");

			break;
			
		case 2:
			
			tmpGORef.transform.parent = BlueListing.transform;
			BlueList.Add(_Name,tmpGORef);
			
			print ("added to blue");
			
			break;
			
			
		case 3:
			
			tmpGORef.transform.parent = GreenListing.transform;
			GreenList.Add(_Name,tmpGORef);
			
			print ("added to green");
			break;
		}
		
		
		UnAssignedListing.GetComponent<UIGrid>().Reposition();
		RedListing.GetComponent<UIGrid>().Reposition();
		BlueListing.GetComponent<UIGrid>().Reposition();
		GreenListing.GetComponent<UIGrid>().Reposition();
		
		tmpGORef.GetComponent<UILabel>().MarkAsChanged();
		
	}

	public void SetPlayerToTeam(string _Name,short _Team)
	{
		
		
		GameObject tmpGO = Instantiate(PlayerNameElement) as GameObject;
		tmpGO.GetComponent<UILabel>().text = _Name;
		
		switch(_Team)
		{
			case 0:
			
				tmpGO.transform.parent = UnAssignedListing.transform;
				tmpGO.transform.localScale= new Vector3(20,20,1);
	
				UnAssignedList.Add(_Name,tmpGO);
			
			break;
			
			case 1:
			
			
				tmpGO.transform.parent = UnAssignedListing.transform;
				tmpGO.transform.localScale= new Vector3(20,20,1);
	
				RedList.Add(_Name,tmpGO);
			
			break;
			
			case 2:
			
			
				tmpGO.transform.parent = UnAssignedListing.transform;
				tmpGO.transform.localScale= new Vector3(20,20,1);
	
				BlueList.Add(_Name,tmpGO);
			
			break;
			
			case 3:
			
				tmpGO.transform.parent = UnAssignedListing.transform;
				tmpGO.transform.localScale= new Vector3(20,20,1);
	
				GreenList.Add(_Name,tmpGO);
			
			break;
		}
	}
	
	public void OrderPlayerList()
	{
		UnAssignedListing.GetComponent<UIGrid>().Reposition();
		RedListing.GetComponent<UIGrid>().Reposition();
		BlueListing.GetComponent<UIGrid>().Reposition();
		GreenListing.GetComponent<UIGrid>().Reposition();
		
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
