using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientFireControl : uLink.MonoBehaviour {
	
	
	public AudioClip laserSound;
	
	//List<Armament> armamentList = new List<Armament>();
	
	Dictionary<int,Armament> armamentList = new Dictionary<int, Armament>();
	public GameObject clientBullet;
	
	
	public void LinkGun (Armament _InLink)
	{
		armamentList.Add(_InLink.id,_InLink);
		print ("Linked");
		
		
	}
	

	// Use this for initialization
	void Start () {
		
		gameObject.name = "" + uLink.Network.player.id;
	
	}
	
	public void SendGunMessage(int _ID,Vector3 _Direction,Vector3 _Position,int _Speed)
	{
		networkView.RPC("GunFired",uLink.RPCMode.Server,_ID,_Direction,_Position,_Speed);
		
		GameObject tmpObj =	(GameObject)Instantiate(clientBullet,_Position,Quaternion.Euler(_Direction));
		ClientBullet cb = tmpObj.GetComponent<ClientBullet>();
		cb.speed = _Speed;
		
		tmpObj.name = "" + uLink.Network.player.id;
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = Camera.mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0));
		
		
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		
		
		foreach(KeyValuePair<int,Armament> pair in armamentList)
		{
			pair.Value.UpdateFacing(ray.GetPoint(40));
			
		}
		
		if(Input.GetButtonDown("Fire1"))
		{
			audio.PlayOneShot(laserSound,0.35f);
			
			
			foreach(KeyValuePair<int,Armament> pair in armamentList)
			{
				if (pair.Value.Fire())
				{
					audio.PlayOneShot(laserSound,1);
				}
			}
		}
		
	
	}

}
