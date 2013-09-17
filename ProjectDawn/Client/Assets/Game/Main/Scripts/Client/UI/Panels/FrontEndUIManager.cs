using UnityEngine;
using System.Collections;

public class FrontEndUIManager : UIManager {
	
	bool loginWindowHidden = true;
	
	public LoginPanel _LoginPanel;
	public GameModePanel _GameModePanel;
	public TopPanel _TopPanel;
	public BottomPanel _BottomPanel;
	public ServerListingPanel _ServerListingPanel;
	
	void Awake ()
	{
		_LoginPanel = transform.FindChild("Camera/Anchor/LoginPanel").gameObject.GetComponent<LoginPanel>();
		_GameModePanel = transform.FindChild("Camera/Anchor/GameModePanel").gameObject.GetComponent<GameModePanel>();
		_TopPanel = transform.FindChild("Camera/Anchor/TopPanel").gameObject.GetComponent<TopPanel>();
		_BottomPanel = transform.FindChild("Camera/Anchor/BottomPanel").gameObject.GetComponent<BottomPanel>();
		_ServerListingPanel = transform.FindChild("Camera/Anchor/ServerListing").gameObject.GetComponent<ServerListingPanel>();
	}
	
	void Start()
	{
		
	}
	
	public void FadeSPlash()
	{
		transform.FindChild("Camera/Anchor/SplashTexture").gameObject.SendMessage("StartFade");
		
	}
	
	public void RemoveSplash()
	{
		
		Destroy(transform.FindChild("Camera/Anchor/SplashTexture").gameObject);
		
	}

	
}
