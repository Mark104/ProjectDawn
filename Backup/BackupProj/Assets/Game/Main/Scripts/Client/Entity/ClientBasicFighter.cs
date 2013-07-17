using UnityEngine;
using System.Collections;

public class ClientBasicFighter : BasicFighter {

	
	public override void OnGUI () {
		
		if(!uLink.Network.isServer)
		{
			GUI.Label(new Rect(200,80,200,20),"Health: " + health);	
		}
	}
	
	public override void Start () {
		
		RadarController	RC = GameObject.FindGameObjectWithTag("RadarObj").GetComponent<RadarController>();
	
		RC.player = this.gameObject;	
		
	}
	
	[RPC]
	public void UpdateHealth (int _Health) {
		print ("Health Updated");
		
		health = _Health;
		
	}
}
