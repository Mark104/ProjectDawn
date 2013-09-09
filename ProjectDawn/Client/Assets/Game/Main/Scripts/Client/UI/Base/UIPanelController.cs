using UnityEngine;
using System.Collections;

public class UIPanelController : MonoBehaviour {
	
	Vector3 hidePosition = new Vector3(0,400,0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void HidePanel()	{
		
		TweenPosition tmpTween = gameObject.AddComponent<TweenPosition>();
		
		tmpTween.from = new Vector3(0,0,0);
		
		tmpTween.to = hidePosition;
		
		tmpTween.callWhenFinished = "HideFinished";
		
		tmpTween.style = UITweener.Style.Once;
		
		tmpTween.method = UITweener.Method.EaseOut;
		
	}
	
	public void ShowPanel()	{
		
		TweenPosition tmpTween = gameObject.AddComponent<TweenPosition>();
		
		tmpTween.from = hidePosition;
		
		tmpTween.to = new Vector3(0,0,0);
		
		tmpTween.callWhenFinished = "HideFinished";
		
		tmpTween.style = UITweener.Style.Once;
		
		tmpTween.method = UITweener.Method.EaseOut;
		
	}
	
	public void SkipPanel()
	{
		
		transform.localPosition = hidePosition;
		
	}
	
	public virtual void HideFInished()
	{
		print ("roar");
		
		
		
		
	}
}
