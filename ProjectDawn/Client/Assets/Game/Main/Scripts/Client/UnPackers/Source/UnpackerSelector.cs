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
	
	public void uLink_OnNetworkInstantiate()
	{
		Default_Unpacker packer = networkView.initialData.Read<Ship_Unpacker>();
	
		if(packer.idKey == 0) // Unpacker is ship type
		{
			Ship_Unpacker tmpPacker = (Ship_Unpacker)packer;
			
			print ("Its a ship, lets start unpacking!");
			
			tmpPacker.Unpack(this.gameObject);
			
		}
		else if (packer.idKey == 1)
		{
			
			
			
		}
	
	
	}
}
