using UnityEngine;
using System.Collections;

public class UIPanelController : MonoBehaviour {
	
	protected Vector3 hidePosition = new Vector3(0,0,0);
	
	protected Vector3 showPosition = new Vector3(0,0,0);
	
	protected bool currentlyHidden = false;
	
	protected float lerpingTime = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ChangeHideState() {
		
		if(currentlyHidden)
		{			
			ShowPanel();
		}
		else
		{
			HidePanel();
		}
		
	}
	
	void HidePanel()	{
		
		TweenPosition tmpTween = gameObject.AddComponent<TweenPosition>();
		
		tmpTween.from = showPosition;
		
		tmpTween.to = hidePosition;
		
		tmpTween.callWhenFinished = "TweenFinished";
		
		tmpTween.style = UITweener.Style.Once;
		
		tmpTween.method = UITweener.Method.EaseOut;
		
		tmpTween.duration = lerpingTime;
		
	}
	
	void ShowPanel()	{
		
		TweenPosition tmpTween = gameObject.AddComponent<TweenPosition>();
		
		tmpTween.from = hidePosition;
		
		tmpTween.to = showPosition;
		
		tmpTween.callWhenFinished = "TweenFinished";
		
		tmpTween.style = UITweener.Style.Once;
		
		tmpTween.method = UITweener.Method.EaseOut;
		
		tmpTween.duration = lerpingTime;
		
		
		
	}
	
	public void SkipPanel()
	{
		if(!currentlyHidden)
		{
			transform.localPosition = hidePosition;
		}
		else
		{
			transform.localPosition = showPosition;
		}
		
		currentlyHidden = !currentlyHidden;
	}
	
	public void TweenFinished()
	{
		print ("Tween FInished");
		currentlyHidden = !currentlyHidden;
		
		Destroy(gameObject.GetComponent<UITweener>());
	}
}
