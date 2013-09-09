using UnityEngine;
using System.Collections;

public class FrontEndUIManager : UIManager {
	
	bool loginWindowHidden = true;
	
	public LoginPanel _LoginPanel;
	
	
	void Start()
	{
		
		_LoginPanel = transform.FindChild("Camera/Anchor/LoginPanel").gameObject.GetComponent<LoginPanel>();
		
	}
	
	
}
