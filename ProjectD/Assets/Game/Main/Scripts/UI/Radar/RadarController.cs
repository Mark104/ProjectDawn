using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RadarController : MonoBehaviour {
	
	
	public int radarRange = 200;
	public GameObject player;
	
	public GameObject LightFighter;
	
	public Dictionary<RadarEntry,GameObject> entrys = new Dictionary<RadarEntry, GameObject>();
	
	
	// Use this for initialization
	void Start () {
	
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(player == null)
		{
			return;
		}
		
		int i = 0;
		
		transform.rotation = player.transform.rotation;
		
		foreach(KeyValuePair<RadarEntry,GameObject> entry in entrys)
		{
			
			Vector3 relativeVector = entry.Key.gameObject.transform.position - player.transform.position;
			
			if(relativeVector.magnitude < radarRange)
			{
				
				if(!entry.Value.activeSelf)
				{
					entry.Value.SetActive(true);
				}
				
				relativeVector.x = (relativeVector.x / radarRange) + 1000;
				relativeVector.y = relativeVector.y / radarRange;
				relativeVector.z = relativeVector.z / radarRange;
				
				
				entry.Value.transform.position = relativeVector;
				
				entry.Value.GetComponent<LineRenderer>().SetPosition(0,entry.Value.transform.position);
				

				relativeVector = relativeVector - transform.position;
				
				Vector3 rayVecX = Vector3.Project(relativeVector,transform.forward);
				
				Vector3 rayVecY = Vector3.Project(relativeVector,transform.right);
				relativeVector = rayVecX + rayVecY;
				
				
				
				relativeVector.x += 1000;
				
				entry.Value.GetComponent<LineRenderer>().SetPosition(1,relativeVector);
			}
			else
			{
				entry.Value.transform.position = Vector3.zero;
				entry.Value.SetActive(false);
			}
			
			i++;
		}
		
	}
	
	public void AddEntry (RadarEntry _Entry)
	{
		print (_Entry.gameObject.name + " added to radar list");
		
		GameObject tmp = Instantiate(LightFighter) as GameObject;
		
		tmp.SetActive(false);
		
		tmp.transform.parent = transform;
		
		
		entrys.Add(_Entry,tmp);
	
	}
}
