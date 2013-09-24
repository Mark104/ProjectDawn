using UnityEngine;
using System.Collections;

public class BasicFighterUnpacker : Unpacker{
	
	GameObject finalGO;
	
	GameObject ship;
	
	int hardpointIlterator = 0;
	
	int moduleIlterator = 0;
	
	
	public override GameObject Unpack(uLink.BitStream _Value,int _UnPackerType)
	{
		uLink.BitStream _StoredShipCode = _Value.ReadBitStream();
		
		bool expectingKey = true;
		
		byte currentKey = 0;

		for(int i = 0;i < 1000;i++)
		{
			if(_StoredShipCode.bytesRemaining == 0)
			{	
				print ("Read finished");
				break;
			}
			
			if(expectingKey)
			{
				expectingKey = false;
				
				currentKey = _StoredShipCode.ReadByte(); 
			}
			else
			{
				expectingKey = true;
				
				ProcessKey(currentKey,_StoredShipCode.ReadInt16());
			}
		}
		
		finalGO = this.gameObject;
		
		return finalGO;
		
	}
	
	
	void ProcessKey(byte _Key,short _Value)
	{
		print ("Reading key " + _Key + " and value is " + _Value);
		
		if(_Key == 2) //Is hardpoint
		{
			if(hardpointIlterator == 0)
			{
				if(unPackerType == Unpacker.TYPE.HANGER)
				{
					
					
					
				}
			}
			
			switch(_Value)
			{
				case 0:
					
					break;
				
				case 1 : // Lets load basiccannon
				
					GameObject tmpObj = Instantiate(Resources.Load("BasicWeapons/BasicCannon")) as GameObject;
				
					tmpObj.transform.parent = ship.transform.Find("Weapon" + hardpointIlterator);
				
					tmpObj.transform.localPosition = Vector3.zero;
				
					tmpObj.transform.localRotation = Quaternion.identity;
				
					break;
			}
			
			hardpointIlterator++;
		}
		else if (_Key == 3) // Is module
		{
			if(hardpointIlterator == 0)
			{
				if(unPackerType == Unpacker.TYPE.HANGER)
				{
					
					
					
				}
			}
			
			switch(_Value)
			{
				case 0:
					
					break;
				
				case 1 : // Lets load basiccannon
				
					break;
			}
			
			moduleIlterator++;
		}
		else if (_Key == 1) // Is engine
		{
			switch(_Value)
			{
				case 0:
					
					break;
					
				case 1: // Lets load the hornet
				
					ship = Instantiate(Resources.Load("BasicFighters/Hornet")) as GameObject;
				
					ship.transform.parent = this.transform;
				
					ship.transform.localPosition = Vector3.zero;
				
					ship.transform.localRotation = Quaternion.identity;
						
					break;
			}
				
			
			
			
		}
		else if (_Key == 4) // is engine
		{
			
			switch(_Value)
			{
				case 0:
					
					break;
				
				case 1 : // Lets load basiccannon
				
					break;
			}
			
		}
	}
	
}
