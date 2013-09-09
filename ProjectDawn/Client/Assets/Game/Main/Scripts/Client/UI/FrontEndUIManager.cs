using UnityEngine;
using System.Collections;

public class FrontEndUIManager : UIManager {
	
	bool loginWindowHidden = true;
	
	LoginPanel _LoginPanel;
	
	
	void Start()
	{
		
		_LoginPanel = transform.FindChild("LoginPanel").gameObject.GetComponent<LoginPanel>();
		//If we do not currently have an account session lets create one, this will store what state
		//the account is currently in and contain callbacks to ulobby
		
		if(GameObject.FindGameObjectWithTag("AccountSession") == null)
		{
			GameObject tmpObj = Instantiate(new GameObject()) as GameObject;
			
			tmpObj.tag = "AccountSession";
			tmpObj.name = "AccountSession";
				
			AccountSession tmpAS = new AccountSession();
			
			tmpObj.AddComponent<AccountSession>();
			tmpObj.AddComponent<Console>();
			
			loginWindowHidden = true;
		}
		else
		{
			//If we already have a session, we must already be loged in!
			
			loginWindowHidden = false;
			
		}
	}
	
	
}
