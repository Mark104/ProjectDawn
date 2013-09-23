using UnityEngine;
using System.Collections;

public class ShowHangerPanel : UIPanelController {

	void Awake () {
		
		hidePosition = new Vector3(0,-300,15);
		showPosition = new Vector3(0,-144.6322f,15);
		currentlyHidden = true;
		
	}
}
