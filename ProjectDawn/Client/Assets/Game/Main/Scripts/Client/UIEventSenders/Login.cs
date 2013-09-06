using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	
	public GameObject Username;
	public GameObject Password;
	

	public void OnClick()
	{
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<ClientFrontEndGC>().Login(Username.GetComponent<UIInput>().text,Password.GetComponent<UIInput>().text);
	
	}
	
}
