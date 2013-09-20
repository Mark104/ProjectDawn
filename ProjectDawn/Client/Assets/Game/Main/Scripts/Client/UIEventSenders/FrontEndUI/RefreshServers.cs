using UnityEngine;
using System.Collections;

public class RefreshServers : MonoBehaviour {

	public void OnClick()
	{
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<FrontEndGC>().RefreshServers();
	
	}
}
