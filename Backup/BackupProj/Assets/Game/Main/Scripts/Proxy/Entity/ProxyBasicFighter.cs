using UnityEngine;
using System.Collections;

public class ProxyBasicFighter : BasicFighter {

	Meter shieldMeter;
	Meter healthMeter;
	
	
	
	
	public override void Start () {
		
		
		shieldMeter = transform.Find("target/ShieldMeter").gameObject.GetComponent<Meter>();
		
		shieldMeter.maxVal = 20;
		shieldMeter.currentVal = 20;
		
		healthMeter = transform.Find("target/HealthMeter").gameObject.GetComponent<Meter>();
		
		healthMeter.maxVal = 20;
		healthMeter.currentVal = 20;
		
		
		
	}
	
	[RPC]
	public void UpdateHealth (int _Health) {
		
		if(healthMeter != null)
		{
		
			healthMeter.currentVal = _Health;
			
			
		}
		health = _Health;
	}
}
