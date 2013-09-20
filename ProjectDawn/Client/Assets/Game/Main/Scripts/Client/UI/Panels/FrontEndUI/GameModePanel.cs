using UnityEngine;
using System.Collections;

public class GameModePanel : UIPanelController {
	
	
	void Awake () {
		
		hidePosition = new Vector3(0,50,0);
		showPosition = new Vector3(0,0,0);
		currentlyHidden = true;
		
	}


}
