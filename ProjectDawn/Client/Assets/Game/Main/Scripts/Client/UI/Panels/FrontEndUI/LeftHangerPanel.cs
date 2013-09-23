using UnityEngine;
using System.Collections;

public class LeftHangerPanel : UIPanelController {

	void Awake () {
		
		hidePosition = new Vector3(-100,0,0);
		showPosition = new Vector3(0,0,0);
		currentlyHidden = true;
		
	}

}
