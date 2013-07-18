using UnityEngine;
using System.Collections;

public class SimpleCamera : MonoBehaviour 
{
    public Transform target = null;
	public Transform targetlook;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
	
	public float smoothTime = 1F;
    public Vector3 velocity = Vector3.zero;
	float fd;

    void LateUpdate () 
	{
		
		
			float rot= ((Input.mousePosition.x/(Screen.width))-0.5f)*3;
			float rot2= (((Input.mousePosition.y/(Screen.height))-0.5f)*3);
			float cs=transform.localPosition.x;
			rot=Mathf.Lerp(cs, rot,0.1f);
			rot2=Mathf.SmoothDamp(transform.localPosition.y-1, rot2, ref fd,0.5f);
			transform.localPosition =new Vector3(rot, rot2+1, -3.5f);
			transform.rotation = Quaternion.LookRotation((target.position + (target.forward * 20) ) - transform.position, target.up);                   
	
     }
	
	
	
	public void SetShip (Transform _incTrans)
	{
		target = _incTrans;
	}
	
	
	
}
