using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;

public class SpaceDoor : MonoBehaviour {


	public int doorID;

	AudioSource audioSource;

	PhotonView photonView;

	bool doorIsOpen = false;

	public delegate void NotifyOpen();
	public NotifyOpen notifyOpen;

	public delegate void NotifyClose();
	public NotifyClose notifyClose;

	// public void AddNofifyOpenListener() {
	// 	notifyOpen += OperateDoor;
	// }

	private void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag != "Hand") return;

		Player collidePlayer = col.gameObject.GetComponentInParent<Player>();
		
		if(collidePlayer == null) return;


		//Only Current Player can control
		if(collidePlayer.playerId != Settings.instance.id) return;

		//does player own that door?
		if(collidePlayer.playerId == doorID) {
			//operate door
			OperateDoor();
		}else {
			//Knock Door
			KnockDoor();
		}

	}

	public void OperateDoor() {
		if(doorIsOpen) {
			photonView.RPC("CloseDoor", PhotonTargets.All);
		}else {
			photonView.RPC("OpenDoor", PhotonTargets.All);
		}
	}

	[PunRPC]
	void OpenDoor() {
		//DO animation
		transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f));
		doorIsOpen = true;
		if(notifyOpen != null)
			notifyOpen();
	}

	[PunRPC]
	void CloseDoor() {
		//DO animation
		transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0f));
		doorIsOpen = false;

		if(notifyClose != null)
			notifyClose();
	}

	
	public void KnockDoor() {
		photonView.RPC("PlayKnockSound", PhotonTargets.All);
	}

	[PunRPC]
	void PlayKnockSound() {
		audioSource.Play();
	}
	///

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource>();
		photonView = GetComponent<PhotonView>();
	}
	

	//Debug:
	void Update() {
		if(Input.GetKeyDown(KeyCode.H)) {
			OperateDoor();
		}
	}
}
