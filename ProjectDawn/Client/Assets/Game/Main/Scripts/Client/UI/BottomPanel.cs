using UnityEngine;
using System.Collections;

public class BottomPanel : UIPanelController {

	void Awake () {
		
		hidePosition = new Vector3(0,-25,0);
		showPosition = new Vector3(0,0,0);
		currentlyHidden = true;
		
	}
}
