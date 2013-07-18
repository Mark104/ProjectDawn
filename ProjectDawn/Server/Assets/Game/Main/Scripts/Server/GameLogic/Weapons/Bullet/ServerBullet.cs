using UnityEngine;
using System.Collections;

public class ServerBullet : MonoBehaviour {
	
	public float life = 7;
	public int speed;
	public float damage = 1;
	float currentLife = 0;

	// Use this for initialization
	void Start () {
		

	
		
		rigidbody.velocity = transform.forward * speed;
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
		currentLife += Time.deltaTime;
		
		if(currentLife > life)
		{
			Destroy(gameObject);	
		}
	
	}
	
	
	void OnTriggerEnter(Collider _Collider)
	{
		if(_Collider.gameObject.name != gameObject.name)
		{
			print ("Hit with "  + damage);
			_Collider.gameObject.SendMessage("Hit",damage);
			Destroy(gameObject);
			
		}
		
	}

}
