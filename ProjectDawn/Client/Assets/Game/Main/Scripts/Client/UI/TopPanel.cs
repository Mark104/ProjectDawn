using UnityEngine;
using System.Collections;

public class TopPanel : UIPanelController {

	void Awake () {
		
		hidePosition = new Vector3(0,30,0);
		showPosition = new Vector3(0,0,0);
		currentlyHidden = true;
		
	}
}
