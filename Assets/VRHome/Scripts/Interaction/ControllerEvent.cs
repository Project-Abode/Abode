using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEvent : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_TrackedController controller;
	
	public GrabInteraction grab;

	public GameObject remote;

	public HandInteraction handHaptic;
	public CoinInteraction coinInteractioin;

	private SteamVR_Controller.Device Controller
	{
		get{ return SteamVR_Controller.Input ((int)trackedObj.index);}
	}

	void Awake()
	{
		Debug.Log("ControllerEvent Awake");
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		//handHaptic.SetUpDevice(trackedObj);

		controller = GetComponent<SteamVR_TrackedController>();
		
		controller.MenuButtonClicked += MenuButtonPressed;
		controller.TriggerClicked += TriggerDown;
		controller.TriggerUnclicked += TriggerUp;
	}

	void OnEnable() {
		if(trackedObj) {
			//handHaptic.SetUpDevice(trackedObj);
		}
		
	}

	void OnDisable() {
		//handHaptic.RemoveDevice();
	}

	void MenuButtonPressed(object sender, ClickedEventArgs e) {
		Debug.Log("Menu Pressed");
		if(PhotonNetwork.isMasterClient) {
			remote.SetActive(!remote.activeSelf);
		}
		
	}

	void TriggerDown(object sender, ClickedEventArgs e) {
		Debug.Log("Trigger Down");
		grab.ControllerTriggerDown(controller);
		coinInteractioin.CreateCoin();
	}

	void TriggerUp(object sender, ClickedEventArgs e) {
		Debug.Log("Trigger Up");
		grab.ControllerTriggerUp(controller);
		coinInteractioin.ReleaseCoin();
	}

	// void Update () {
		
	// 	if (Controller.GetHairTriggerDown ()) {
	// 		Debug.Log("Trigger Down");
	// 		grab.ControllerTriggerDown(controller);
	// 	}

	// 	if (Controller.GetHairTriggerUp ()) {
	// 		Debug.Log("Trigger UP");
	// 		grab.ControllerTriggerUp(controller);
	// 	}

	// }



}
