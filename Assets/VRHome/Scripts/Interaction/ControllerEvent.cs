using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEvent : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	
	public GrabInteraction grab;

	private SteamVR_Controller.Device Controller
	{
		get{ return SteamVR_Controller.Input ((int)trackedObj.index);}
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}


	void Update () {
		
		if (Controller.GetHairTriggerDown ()) {
            grab.ControllerTriggerDown();
		}

		if (Controller.GetHairTriggerUp ()) {
         	grab.ControllerTriggerUp();
		}
	}



}
