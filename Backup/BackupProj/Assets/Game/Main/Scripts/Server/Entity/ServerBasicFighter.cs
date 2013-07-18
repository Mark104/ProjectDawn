using UnityEngine;
using System.Collections;

public class ServerBasicFighter : BasicFighter {
	
	public override void Start () 
	{
			
		networkView.RPC("UpdateHealth",uLink.RPCMode.All,(int)health);	
			
	}
	
	
	
	public override void Hit (float _Damage)
	{
		if(health == 1)
		{
			GameObject.FindGameObjectWithTag("GameController").GetComponent<MainServerController>().SendMessage("RespawnPlayer",networkView.owner);
		}
		
		base.Hit(_Damage);
	
		networkView.RPC("UpdateHealth",uLink.RPCMode.All,(int)health);
	}
}
