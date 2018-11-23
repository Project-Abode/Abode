using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEvent : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_TrackedController controller;
	
	//public GrabInteraction grab;
	

	//public HandInteraction handHaptic;
	//public CoinInteraction coinInteractioin;

	private SteamVR_Controller.Device Controller
	{
		get{ return SteamVR_Controller.Input ((int)trackedObj.index);}
	}


	public delegate void OnTriggerDown();
	public OnTriggerDown onTriggerDown;

	public delegate void OnTriggerUp();
	public OnTriggerUp onTriggerUp;

	public delegate void OnPadClicked();
	public OnPadClicked onPadClicked;


	public SimpleGrab simpleGrab;

	public GameObject remote;


	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		controller = GetComponent<SteamVR_TrackedController>();
		
		controller.PadClicked += PadClicked;
		controller.MenuButtonClicked += MenuButtonPressed;
		controller.TriggerClicked += TriggerDown;
		controller.TriggerUnclicked += TriggerUp;

		simpleGrab.enabled = true;

	}

	// void OnEnable() {
	// 	if(trackedObj) {
	// 		//handHaptic.SetUpDevice(trackedObj);
	// 	}
		
	// }

	// void OnDisable() {
	// 	//handHaptic.RemoveDevice();
	// }

	void PadClicked(object sender, ClickedEventArgs e) {
		Debug.Log("Pad Clicked");

		remote.SetActive(!remote.activeSelf);

		if(onPadClicked!=null)
			onPadClicked();

	}

	void MenuButtonPressed(object sender, ClickedEventArgs e) {
		Debug.Log("Menu Pressed");
		//if(PhotonNetwork.isMasterClient) {
		//remote.SetActive(!remote.activeSelf);
		//}
		
	}

	void TriggerDown(object sender, ClickedEventArgs e) {
		// Debug.Log("Trigger Down");
		// grab.ControllerTriggerDown(controller);
		// coinInteractioin.CreateCoin();

		
		if(onTriggerDown!=null)
			onTriggerDown();

	}

	void TriggerUp(object sender, ClickedEventArgs e) {
		// Debug.Log("Trigger Up");
		// grab.ControllerTriggerUp(controller);
		// coinInteractioin.ReleaseCoin();

		if(onTriggerUp!=null)
			onTriggerUp();
	}


}
