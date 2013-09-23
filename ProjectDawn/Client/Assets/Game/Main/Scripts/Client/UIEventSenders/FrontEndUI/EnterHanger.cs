using UnityEngine;
using System.Collections;

public class EnterHanger : MonoBehaviour {
	
	bool isHangerMode = false;
	
	void ChangeMode()
	{
		
		isHangerMode = !isHangerMode;
		if(isHangerMode)
		{
			gameObject.transform.Find("Label").GetComponent<UILabel>().text = "Return";	
		}
		else
		{
			gameObject.transform.Find("Label").GetComponent<UILabel>().text = "Hanger";
		}
	}

	public void OnClick()
	{
		ChangeMode();
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<FrontEndGC>().EnterHangerMode(isHangerMode);

		
		
	
	}
	
	
}
