using UnityEngine;
using System.Collections;

public class RadarEntry : MonoBehaviour {
	
	RadarController RC;

	// Use this for initialization
	public virtual void Start () {
		
		RC = GameObject.FindGameObjectWithTag("RadarController").GetComponent<RadarController>();
	
		RC.AddEntry(this);
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}
}
