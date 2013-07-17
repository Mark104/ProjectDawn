using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Console : MonoBehaviour {
	
	public int maximumEntries; // We have a maximum amount of entries that as display in the console
	
	List<string> msgList = new List<string>();
	
	Vector2 scrollPosition;
	
	int inc = 0;
		
	// Use this for initialization
	void Start () {
		

	
	}
	
	public void AddMessage(string _MsgToAdd) {
		
		Debug.Log(_MsgToAdd);
		
		msgList.Add(_MsgToAdd);
		
		inc++;
		
		if(msgList.Count > maximumEntries)
		{
			
			msgList.RemoveAt(0);

		}
		else
		{
			
			return;
		}
	}	
	
	void OnGUI () {
		
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(600), GUILayout.Height(1000));
		
		foreach(string roundString in msgList)
		{				
       	 	GUILayout.Label(roundString);		
		}
		    
		GUILayout.EndScrollView();
	}
}
