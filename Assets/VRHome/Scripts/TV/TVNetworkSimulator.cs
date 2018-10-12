using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVNetworkSimulator : MonoBehaviour {
	//Use for temp network simulation

	public TVController tv;
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)) {
			//Guest accept invitation
			tv.GuestAccept();
		}
	}

	



}
