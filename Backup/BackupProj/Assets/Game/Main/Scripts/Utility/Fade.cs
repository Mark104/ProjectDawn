using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
	
	public float time = 2;
	
	public enum FadeType {FadeToBlackAndOut};
	
	public FadeType fadeMethod;
	
	Color currentCol = Color.white;
	
	bool isFading = false;

	
	private float fadeStartTime;

	// Use this for initialization
	void Start () {
		
		currentCol.a = 255;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(isFading)
		{
			float currentLerpStep = Time.time - fadeStartTime;
			
			
			
			if(currentLerpStep <= 1)
			{
				currentCol.r = Mathf.Lerp(1,0,currentLerpStep);
				currentCol.g = Mathf.Lerp(1,0,currentLerpStep);
				currentCol.b = Mathf.Lerp(1,0,currentLerpStep);
			}
			else
			{
				currentCol.a = Mathf.Lerp(1,0,-1 + currentLerpStep);
				if(currentLerpStep > 2)
				{
					isFading = false;
				}
			}

			
			renderer.material.SetColor("_Color",currentCol);
			
		}
	
	}
	
	public void StartFade ()
	{
		fadeStartTime = Time.time;
		
		isFading = true;
	}
	
	public void StartFade (float _Time)
	{
		fadeStartTime = Time.time;
		
		isFading = true;
	}
}
