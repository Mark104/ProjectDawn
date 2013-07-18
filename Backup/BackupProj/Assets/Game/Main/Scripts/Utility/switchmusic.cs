using UnityEngine;
using System.Collections;

public class switchmusic : MonoBehaviour {
	
	
	public AudioClip newSound;
	
	private float switchTime;
	
	bool isSwitching = false;
	
	bool musicSwitched = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(isSwitching)
		{
			float lerpVal = Time.time - switchTime;
			
			
			if(lerpVal <= 1)
			{	
				audio.volume = Mathf.Lerp(0.2f,0,lerpVal);
			}
			else
			{
				if(!musicSwitched)
				{
					audio.clip = newSound;
					audio.Play();
					musicSwitched = true;	
				}
				
				audio.volume = Mathf.Lerp(0,0.2f,-1 + (lerpVal * 0.5f));
				
				
				if(lerpVal > 3)
				{
					isSwitching = false;
				}
				
			}
			
		}
	
	}
	
	public void SwitchSound ()
	{
		switchTime = Time.time;
		isSwitching = true;
	}
}
