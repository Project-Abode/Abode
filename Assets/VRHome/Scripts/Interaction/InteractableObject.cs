using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;


public class InteractableObject : MonoBehaviour {

	// Use this for initialization

	public bool locked = false;

	// [SerializeField]
    // PhotonView _photonView;


	void Awake() {

		var rb = GetComponent<Rigidbody>();
		if(!PhotonNetwork.isMasterClient) {
			Destroy(rb);
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

	// [PunRPC]
    // public void Holded()
    // {
    //    locked = true;
    // }

	
	// [PunRPC]
    // public void Released()
    // {
    //    locked = false;
    // }


}
