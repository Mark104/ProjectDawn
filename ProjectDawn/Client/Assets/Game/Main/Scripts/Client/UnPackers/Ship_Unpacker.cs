using UnityEngine;
using uLink;
using System.Collections;
using System.Collections.Generic;

public class Ship_Unpacker : Default_Unpacker {
	
	public byte hullType;
	
	public byte[] weaponListing;
	
	public override void Unpack(GameObject rootObj)
	{
		if(hullType == 0)
		{
			GameObject tmpObj =	 GameObject.Instantiate(Resources.Load("GameAssets/Hulls/hull1"),rootObj.transform.position,rootObj.transform.rotation) as GameObject;
			
			tmpObj.transform.parent = rootObj.transform;
			
			string nameToFind = "wpnSlot1";
			Transform currentTransform;
			int i = 0;
			do
			{
				
				currentTransform = tmpObj.transform.Find(nameToFind);
				
				if(currentTransform != null)
				{
					if(weaponListing[i] == 0)
					{
						GameObject tmpWpnObj =	 GameObject.Instantiate(Resources.Load("GameAssets/Weapons/BasicCannon"),currentTransform.position,currentTransform.rotation) as GameObject;
			
						tmpWpnObj.transform.parent = currentTransform;
					}
					else if (weaponListing[i] == 1)
					{
						
							
					}
					
					
					Debug.Log("Found " + i + " weapon");
					
					i++;
					
					nameToFind = "wpnSlot" + i;
				}
					
			}while(currentTransform != null);
			
		}
		else if (hullType == 1)
		{
			
			
			
			
		}
		
		
	}

}
