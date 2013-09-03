using UnityEngine;
using System.Collections;

public class GameController : uLink.MonoBehaviour {
	
    public virtual void OnHover (bool isOver,short id){}
	
    public virtual void OnPress (bool isDown,short id){}
	
    public virtual void OnClick(short id){}
	
    public virtual void OnDoubleClick (short id){}
	
    public virtual void OnSelect (bool selected,short id){}
	
    public virtual void OnDrag (Vector2 delta,short id){}
	
    public virtual void OnDrop (GameObject drag,short id){}
	
    public virtual void OnInput (string text,short id){}
	
    public virtual void OnTooltip (bool show,short id){}
	
    public virtual void OnScroll (float delta,short id){}
	
    public virtual void OnKey (KeyCode key,short id){}
}
