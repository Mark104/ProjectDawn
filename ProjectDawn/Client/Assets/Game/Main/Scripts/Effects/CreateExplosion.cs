using UnityEngine;
using System.Collections;

public class CreateExplosion : MonoBehaviour {
	
	
	public GameObject ps;

	void OnDestroy()
	{
		if (!Application.isEditor)
		{
		
			Instantiate(ps,transform.localPosition,transform.localRotation);
		}

	}
}
