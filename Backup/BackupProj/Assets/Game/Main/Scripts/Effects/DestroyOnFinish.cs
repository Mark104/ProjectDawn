using UnityEngine;
using System.Collections;

public class DestroyOnFinish : MonoBehaviour {
	
	
	public float life = 8;
	
	float timeToDestroy;

	// Use this for initialization
	void Start () {
		
		timeToDestroy = Time.time +  life;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(Time.time > timeToDestroy)
		{
			Destroy(gameObject);
		}
	
	}
}
