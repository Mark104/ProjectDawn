using UnityEngine;
using uLink;
using uLobby;
using System.Collections;

public class ShipBuilder : uLink.MonoBehaviour {
	
	string shipCode;
	
	uLobby.Account account;
	
	GameObject StoredShip = null;
	
	ShipBuilder (string _ShipCode,uLobby.Account _Account)
	{
		shipCode = _ShipCode;
		account = _Account;
		
	}
	
	// Use this for initialization
	void Start () {
	
		shipCode = "-hlwk-";
		
		CreateShip();
		
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
							
									switch(coderHold[0])
									{
										case 'a':
								
											StoredShip = Resources.Load("Prefabs/Ships/Hull1") as GameObject;
										
										break;
								
										case 'b':
								
											StoredShip = Resources.Load("Prefabs/Ships/Hull2") as GameObject;
										
										break;
								
										case 'c':
								
											StoredShip = Resources.Load("Prefabs/Ships/Hull3") as GameObject;
										
										break;
									}

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

	
	
	// Update is called once per frame
	void Update () {
	
	}
}
