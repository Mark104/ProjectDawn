using UnityEngine;
using System.Collections;

public class CustomMatch : MonoBehaviour {

	public void OnClick()
	{
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<FrontEndGC>().ShowServers();
	
	}
}
