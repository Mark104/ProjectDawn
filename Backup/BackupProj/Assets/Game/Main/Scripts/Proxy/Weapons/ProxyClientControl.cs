using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProxyClientControl : uLink.MonoBehaviour {
	
	//List<Armament> armamentList = new List<Armament>();
	
	Dictionary<int,Armament> armamentList = new Dictionary<int, Armament>();
	public GameObject clientBullet;
	
	public AudioClip laserSound;
	
	public void LinkGun (Armament _InLink)
	{
		armamentList.Add(_InLink.id,_InLink);
		print ("Linked");
		
		
	}
	
	// Use this for initialization
	void Start () {
		
		gameObject.name = "" + networkView.owner.id;
	
	}
		
	[RPC]
	public void GunFired (int _Id,Vector3 _Direction, Vector3 _Position, int _Velocity,int _PlayerID)
	{
		audio.PlayOneShot(laserSound,0.35f);
		
		print (_PlayerID);
		GameObject tmpObj =	(GameObject)Instantiate(clientBullet,_Position,Quaternion.Euler(_Direction));
		ClientBullet cb = tmpObj.GetComponent<ClientBullet>();
		cb.speed = _Velocity;
		
		tmpObj.transform.Translate(new Vector3(0,0,_Velocity * (uLink.Network.player.averagePing * 0.0001f)));
			
		tmpObj.name = "" + _PlayerID;
		
		//networkView.RPC("GunFired",uLink.RPCMode.Server,_Id,_Direction,_Position,_Velocity);
		
	}

}
