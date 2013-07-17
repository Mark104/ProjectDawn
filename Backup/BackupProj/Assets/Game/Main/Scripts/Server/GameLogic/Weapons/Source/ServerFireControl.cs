using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ServerFireControl : uLink.MonoBehaviour {
	
	
	public GameObject bullet;
	
	List<Armament> armamentList = new List<Armament>();
	
	
	public void LinkGun (Armament _InLink)
	{
		armamentList.Add(_InLink);
		print ("Linked");
		
		
	}
	
	
	
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[RPC]
	public void GunFired (int _Id,Vector3 _Direction, Vector3 _Position, int _Velocity,uLink.NetworkMessageInfo info)
	{
	
		GameObject tmpObj =	(GameObject)Instantiate(bullet,_Position,Quaternion.Euler(_Direction));
		ServerBullet sb = tmpObj.GetComponent<ServerBullet>();
		
		tmpObj.transform.Translate(new Vector3(0,0,_Velocity * (info.sender.averagePing * 0.0001f)));
			
		tmpObj.name = "" + gameObject.name;
		sb.speed = _Velocity;
		
		
		networkView.RPC("GunFired",uLink.RPCMode.AllExceptOwner,_Id,_Direction,tmpObj.transform.position,_Velocity,int.Parse(gameObject.name));
		
	}
}
