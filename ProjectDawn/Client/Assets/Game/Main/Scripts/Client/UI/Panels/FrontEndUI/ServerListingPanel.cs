using UnityEngine;
using System.Collections;

public class ServerListingPanel : UIPanelController {

	void Awake () {
		
		hidePosition = new Vector3(670,0,0);
		showPosition = new Vector3(0,0,0);
		currentlyHidden = true;
		
	}
}
