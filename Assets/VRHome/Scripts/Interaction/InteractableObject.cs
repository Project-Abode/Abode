using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;


public class InteractableObject : MonoBehaviour {

	// Use this for initialization

	public bool locked = false;

	void Awake() {

		var rb = GetComponent<Rigidbody>();
		if(PhotonNetwork.isMasterClient) {
            //Destroy(rb);
            rb.isKinematic = false;
            rb.useGravity = true;
		}

	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			if(!locked) {
				HoldObject();
			}
		}

		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			if(locked) {
				ReleaseObject();
			}
		}
	}

	public void HoldObject() {

		//_photonView.RPC("Holded", PhotonTargets.All);
		locked = true;
	}

	public void ReleaseObject() {
		//_photonView.RPC("Released", PhotonTargets.All);
		locked = false;
	}

}
