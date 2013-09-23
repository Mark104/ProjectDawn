using UnityEngine;
using System.Collections;

public class JoinTeam : MonoBehaviour {
	
	public byte myTeam;

	// Use this for initialization
	public void OnClick()
	{
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<SpaceBattlefieldGC>().JoinTeam(myTeam);
	
	}
}
