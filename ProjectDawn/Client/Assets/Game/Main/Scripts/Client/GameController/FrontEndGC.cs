using UnityEngine;
using System.Collections;

public class FrontEndGC : GameController {
	
	
	FrontEndUIManager UIManager;
	AccountSession AS;
	
	public void ShowLoginPanel()
	{
		UIManager._LoginPanel.ShowPanel();
	}
	
	public void HideLoginPanel()
	{
		UIManager._LoginPanel.HidePanel();
	}
	
	public void SkipLoginPhase()
	{
		UIManager._LoginPanel.SkipPanel();
	}
	
	public void Login (string _Username, string _Password)
	{
		AS.Login(_Username,_Password);
	}
	

	// Use this for initialization
	void Start () {
	
		UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<FrontEndUIManager>();
		
		//If we do not currently have an account session lets create one, this will store what state
		//the account is currently in and contain callbacks to ulobby
		
		GameObject actSessionObj = GameObject.FindGameObjectWithTag("AccountSession");
		
		
		if(actSessionObj == null)
		{
			GameObject tmpObj = Instantiate(new GameObject()) as GameObject;
			
			tmpObj.tag = "AccountSession";
			tmpObj.name = "AccountSession";
				
			AS = tmpObj.AddComponent<AccountSession>();
			AS._FrontEndGC = this;
			tmpObj.AddComponent<Console>();
		
		
		}
		else
		{
			//If we already have a session, we must already be loged in!
			AS.GetComponent<AccountSession>()._FrontEndGC = this;	
			SkipLoginPhase();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
