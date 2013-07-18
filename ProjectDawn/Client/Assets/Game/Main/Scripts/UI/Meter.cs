using UnityEngine;
using System.Collections;

public class Meter : MonoBehaviour {
	
	public float maxVal;
	public float currentVal;
	
	float startingVal;

	// Use this for initialization
	void Start () {
		
		startingVal = transform.localScale.x;
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(maxVal != 0)
		{
			transform.localScale = new Vector3(Mathf.Lerp(0,startingVal,currentVal / maxVal),transform.localScale.y,transform.localScale.z); 
		}
	}
}
