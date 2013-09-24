using UnityEngine;
using System.Collections;

public class Unpacker : MonoBehaviour {
	
	public enum TYPE{
		
		OWNER,
		PROXY,
		HANGER
	}
	
	protected	TYPE unPackerType;
	
	public virtual GameObject Unpack (uLink.BitStream _Value,int _UnPackerType)
	{
		GameObject finalGO = this.gameObject;
		
		
		
			unPackerType = (TYPE)_UnPackerType;
		
		return finalGO;
	}
}
