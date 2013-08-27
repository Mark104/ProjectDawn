using UnityEngine;
using System.Collections;

public class ClientMaster : uLink.MonoBehaviour {
	
	bool hasReturnedToHanger = false;
	
	public bool ReturningToHanger()
	{
		hasReturnedToHanger = true;
		return hasReturnedToHanger;
	}
	
	
	public bool IsReturningToHanger()
	{
		bool currentReturn = hasReturnedToHanger;
		hasReturnedToHanger = false;
		return currentReturn;
	}

	void Awake () {
		
			DontDestroyOnLoad(this);
		
	}
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
