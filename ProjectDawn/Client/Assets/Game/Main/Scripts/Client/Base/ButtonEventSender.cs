using UnityEngine;
using System.Collections;

public class ButtonEventSender : MonoBehaviour {

	GameController GC;
	
	public short id;
	
	public void Start ()
	{
		GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
    public void OnHover (bool isOver)
	{
		GC.OnHover(isOver,id);
	}
	
    public void OnPress (bool isDown)
	{
		GC.OnPress(isDown,id);		
	}
	
    public void OnClick()
	{
		GC.OnClick(id);
	}
	
    public void OnDoubleClick ()
	{
		GC.OnDoubleClick(id);
	}
	
    public void OnSelect (bool selected)
	{
		GC.OnSelect(selected,id);
	}
	
    public void OnDrag (Vector2 delta)
	{
		GC.OnDrag(delta,id);
	}
	
    public void OnDrop (GameObject drag)
	{
		GC.OnDrop(drag,id);
	}
	
    public void OnInput (string text)
	{
		GC.OnInput(text,id);
	}
	
    public void OnTooltip (bool show)
	{
		GC.OnTooltip(show,id);
	}
	
    public void OnScroll (float delta)
	{
		GC.OnScroll(delta,id);
	}
	
    public void OnKey (KeyCode key)
	{
		GC.OnKey(key,id);
	}
}
