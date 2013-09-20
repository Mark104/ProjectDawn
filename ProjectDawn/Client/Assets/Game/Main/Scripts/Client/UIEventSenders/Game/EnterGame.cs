using UnityEngine;
using System.Collections;

public class EnterGame : MonoBehaviour {

	public void OnClick()
	{
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<SpaceBattlefieldGC>().EnterGame();
	
	}
}
