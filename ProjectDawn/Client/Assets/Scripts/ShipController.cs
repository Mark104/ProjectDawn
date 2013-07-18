using UnityEngine;
using System.Collections;

public class ShipController : uLink.MonoBehaviour 
{
	public Transform m_ShipMesh = null;
	
	
	public GameObject m_Cam;
	
	Vector3 storedForce;

	// Distance to the look point 
	public float m_DistanceToLookPoint = 25.0f;
	// Damping 
	public float m_Damping = 3.0f;
	// Banking angle
	public float m_BankingAngle = 75.0f;
	// Ship Force 
	public float m_Force = 10.0f;
	
	public float m_MaxForce = 20.0f;
	public float m_MinForce = 10.0f;
	
	public float m_MaxFOV = 120.0f;
	public float m_MinFOV = 60.0f;
	
	float nextSend = 0.1f;
	
	// Distance form camer to the ship
	private float m_ShipCamOffset = 0;
	// Point to look at
	public Vector3 m_PointLookAt;
	// Rotation 
	private Quaternion rotation;
	
	private float _CurrentForce = 0;
	private float _CurrentFOV = 0;
	
	float test = 1.0f;
	float test1 = 1.0f;
	// Use this for initialization
	void Start () 
	{
	
	//	GameObject tmpObj = Instantiate(m_Cam,new Vector2(0,0),Quaternion.identity) as GameObject;
		
	//	tmpObj.SendMessage("SetShip",transform);
		
		_CurrentFOV = m_MinFOV;
	}
	
	void OnGUI ()
	{
		GUI.Label(new Rect(200,40,200,20),"Speed: " + m_Force);
		
	}
	
	void Update ()
	{
		if(Input.GetButtonDown("IncreaseSpeed"))
		{
			if (m_Force < 20)
			{
				m_Force += 5;	
			}
		}
		
		if(Input.GetButtonDown("DecreaseSpeed"))
		{
			if (m_Force > 0)
			{
				m_Force -= 5;
			}
		}
	}
	
	void LateUpdate ()
	{
		nextSend += Time.deltaTime;
		
		// Get the distance betwen camera Z and ship 
		m_ShipCamOffset = this.transform.position.z - Camera.main.transform.position.z;
		
		// Get point to look at
		m_PointLookAt = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ShipCamOffset+m_DistanceToLookPoint));
		
		
		// Rotate the object with a damping added 
		rotation = Quaternion.LookRotation(m_PointLookAt- transform.position, transform.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_Damping);		
		
		
		float rot= -((Input.mousePosition.x/(Screen.width))-0.5f)*m_BankingAngle;
		m_ShipMesh.localRotation=Quaternion.Euler(0,0, rot);
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		
		
		this.rigidbody.AddForce(this.transform.forward * m_Force);
		storedForce += transform.forward * m_Force;
		
		
		
		
		
		/* Old Boost Code
		if(Input.GetMouseButton(1))
		{
			_CurrentForce+=0.5f;
			test -= 0.01f;
			test1 = 1;
			if(m_Force <= m_MaxForce)
			{
				m_Force += _CurrentForce;
				
			}
			
			_CurrentFOV = Mathf.Lerp(m_MaxFOV, m_MinFOV, test);
			Camera.mainCamera.fieldOfView = _CurrentFOV;
			Debug.LogError(_CurrentFOV);
		}
		else
		{
			if(m_Force>=m_MinForce)
			{
				
				_CurrentForce = Mathf.Lerp(m_Force, m_MinForce, Time.smoothDeltaTime);
				m_Force = _CurrentForce;
				
				
			}
			
			if(Camera.mainCamera.fieldOfView > 60)
			{
				test1-=0.01f;
				test = 1;
				_CurrentFOV = Mathf.Lerp(m_MinFOV, m_MaxFOV,test1);
				
				Camera.mainCamera.fieldOfView = _CurrentFOV;
			}
		}
		*/
		
		
		if(nextSend > 0.1f)
		{
			
			networkView.RPC("InputRecived",uLink.RPCMode.Server,this.storedForce,transform.rotation);
			storedForce = new Vector3(0,0,0);
			nextSend = 0;
			
		}
		
		
		
	}
	
	
	
}
