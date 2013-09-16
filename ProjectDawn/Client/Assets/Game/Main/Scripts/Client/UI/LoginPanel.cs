using UnityEngine;
using System.Collections;

public class LoginPanel : UIPanelController {
	
	
	void Awake () {
		
		hidePosition = new Vector3(0,400,0);
		showPosition = new Vector3(0,0,0);
		currentlyHidden = true;
		
	}


}
