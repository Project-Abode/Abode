using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

	public bool isDoorOpen = false;

	private PhotonView photonView;

	public GameObject doorObject;

	public ElevatorButton button;

	void Awake() {
		photonView = GetComponent<PhotonView>();
		button.notifyTouched += OnButtonClicked;
	}

	void OnButtonClicked() {
		if(isDoorOpen) {
			CloseDoor();
		}else {
			OpenDoor();
		}
	}


	public void CloseDoor() {
		photonView.RPC("CloseDoorRPC", PhotonTargets.All);
	}

	//todo: AudioSource

	[PunRPC]
	void CloseDoorRPC() {
		doorObject.SetActive(true);
		isDoorOpen = false;

	}


	public void OpenDoor() {
		photonView.RPC("OpenDoorRPC", PhotonTargets.All);
	}

	[PunRPC]
	void OpenDoorRPC() {
		doorObject.SetActive(false);
		isDoorOpen = true;
	}

}
