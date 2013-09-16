using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour {
	
	
	public GameObject chump;
	public GameObject chumpPette;
	float hSliderValue = 1;
	float animationSpeed = 1;
	float autopitchValue = 1;
	
	bool manualPitch;

	// Use this for initialization
	void Start () {
		
		
	}
		
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void Shoot() {
		
		chump.particleSystem.Emit(30);
		print ("Roar");
		audio.Play();
		
	}
	
	void OnGUI() {
	
		manualPitch = GUI.Toggle(new Rect (25, 25, 100, 30),manualPitch,"ManualPitch");
		
		if(manualPitch)		
		{
			hSliderValue = GUI.HorizontalSlider (new Rect (25, 75, 100, 30), hSliderValue, 0.0f, 1.0f);
			audio.pitch = hSliderValue;
			chumpPette.audio.pitch = hSliderValue;
		}
		else
		{
			audio.pitch = autopitchValue;
			chumpPette.audio.pitch = autopitchValue;
			
		}

		animationSpeed	= GUI.HorizontalSlider (new Rect (25, 55, 100, 30), animationSpeed, 1, 10);
		
		autopitchValue = 1 + (animationSpeed / 10);
		
		animation["Take 002"].speed = animationSpeed;
		
		
		
	}
}
