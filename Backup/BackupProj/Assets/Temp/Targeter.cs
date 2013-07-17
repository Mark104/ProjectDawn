using UnityEngine;
using System.Collections;

public class Targeter : MonoBehaviour {
	
	public GameObject gameObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 pos = gameObj.transform.position;
		Vector3 vt = Camera.main.WorldToViewportPoint(pos);

		
		float dist = (pos - Camera.main.transform.position).magnitude;

		// Clamp to the edges of the screen
		vt.x = Mathf.Clamp(vt.x,0,1);
		vt.y = Mathf.Clamp(vt.y,0,1);
		
		
		if(vt.z < 0)
		{
			vt.z = 0;	
		}
		else
		{
			vt.z = Mathf.Clamp(dist,2,26);
		}

		// Convert back to world
		pos = Camera.main.ViewportToWorldPoint(vt);
		
		transform.position = pos;
		transform.rotation = Camera.main.transform.rotation;


	
	}
}
