using UnityEngine;
using System.Collections;

public class SpaceBattlefield : MonoBehaviour {

	public void OnClick()
	{
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<FrontEndGC>().GameTypeSelection(0);
	
	}
}
