using UnityEngine;
using uLink;
using uLobby;
using System.Collections;

public class ShipBuilder : uLink.MonoBehaviour {
	
	string shipCode;
	
	uLobby.Account account;
	
	ShipBuilder (string _ShipCode,uLobby.Account _Account)
	{
		shipCode = _ShipCode;
		account = _Account;
	}
	
	void CreateShip()
	{
		//First lets parse the string
		
		string coderHold = "";
		
		char keyChar = 'a';
		
		bool readingKey = false;
		
		foreach(char currentLetter in shipCode)
		{
			if(!readingKey)
			{
				
				switch(currentLetter) // Info / Key starter
				{
					case '-':
					
						readingKey = true;
					
						if(coderHold.Length > 0)
						{
							switch(keyChar)
							{
								case 'h':
								
							
								break;
							
							}
							
							
						}
					
						coderHold = "";
					
					
					break;
					
					default:
					
						coderHold += currentLetter;
					
					break;
				}
			
			}
			else
			{
				
				keyChar = currentLetter;
				readingKey= false;
				
				
				
			}
		
		}
		
	}
	
	void EditShip()
	{
		
		
		
	}
	
	void SaveToDatabase()
	{
			
		
	}
	
	/*
	GameObject GetShip()
	{
		
		
		
	}
	*/

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
