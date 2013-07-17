using UnityEngine;
using System.Collections;
using uLink;

public class Entity : uLink.MonoBehaviour {
	
	public float health = 10;

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}
	
	public virtual void OnGUI () {
		
		
	}
	
	
	public virtual void Hit (float _Damage)
	{
		print ("I Took " + _Damage);
		
		health -= _Damage;
		
		if(health <= 0)
		{
			uLink.Network.Destroy(networkView);
			Destroy(gameObject);
		}
		
	}
}
