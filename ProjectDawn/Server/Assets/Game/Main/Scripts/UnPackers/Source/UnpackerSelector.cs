using UnityEngine;
using uLink;
using System.Collections;

public class UnpackerSelector : uLink.MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void uLink_OnNetworkInstantiate(Hashtable _Data)
	{
		print (_Data["msg"].ToString());

		
		
		
	}
}
