using UnityEngine;
using System.Collections;

public class Armament : MonoBehaviour {
	
	public ClientFireControl CFC;
	
	
	public int id;
	
	public virtual bool Fire () {	
		
		return false;
	}
	
	public virtual void UpdateFacing (Vector3 _Pos) {	
		
			
	}
	
	public virtual string Status () {
			
		return "test";
		
	}

	// Use this for initialization
	public virtual void Start () {
		
		transform.root.gameObject.SendMessage("LinkGun",this);
		CFC = transform.root.GetComponent<ClientFireControl>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
