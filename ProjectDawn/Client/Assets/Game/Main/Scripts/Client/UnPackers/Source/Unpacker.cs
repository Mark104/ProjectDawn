using UnityEngine;
using System.Collections;

public class Unpacker : MonoBehaviour {
	
	public enum TYPE{
		
		OWNER,
		PROXY,
		HANGER
	}
	
	TYPE unPackerType;
	
	public virtual void Unpack (string _Value,int _UnPackerType)
	{
		unPackerType = (TYPE)_UnPackerType;
		
	}
}
