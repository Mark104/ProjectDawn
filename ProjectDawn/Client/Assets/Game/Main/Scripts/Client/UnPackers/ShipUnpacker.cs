using UnityEngine;
using System.Collections;

public class ShipUnpacker : Unpacker {
	
	bool expectingKey = true;
	char key;
	byte keyValue = 0;
	
	
	

	public override void Unpack (string _Value,int _UnPackerType)
	{
		base.Unpack(_Value,_UnPackerType);
		
		foreach(char letter in _Value)
		{
			if(expectingKey)
			{
				key = letter;
			}
			else
			{
				if(letter != '-')
				{
					keyValue = (byte)letter;
				}
				else
				{
					ProcessKey();
					expectingKey = true;	
				}
			}
		}
		
	}
	
	private void ProcessKey()
	{
		if(key == 'W') // Weapons
		{
			
			
		}
		else if (key == 'M') // Modules
		{
			
			
			
		}
		else if (key == 'E') // Enchancements
		{
			
			
		}
		else if (key == 'T') //Type
		{
			
			
			
		}
		else if (key == 'H') //Hull
		{
			
			
			
		}
		else if (key == 'N') // Engine
		{
			
			
			
		}		
	}
}
