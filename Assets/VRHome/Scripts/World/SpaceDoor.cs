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
		if(!collidePlayer.playerId.Equals(Settings.instance.id)) 
			return;

		//does player own that door?
		if(collidePlayer.playerId.Equals(doorID)) {
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

	public void OnlyCloseDoor(){
		if(doorIsOpen) {
			photonView.RPC("CloseDoor", PhotonTargets.All);
		}
	}

	public void OnlyOpenDoor(){
		if(!doorIsOpen) {
			photonView.RPC("OpenDoor", PhotonTargets.All);
		}
	}


	public Coroutine curDoorAnim;
	
	[PunRPC]
	void OpenDoor() {
		//DO animation
		doorIsOpen = true;
		if(curDoorAnim!= null) {
			StopCoroutine(curDoorAnim);
		}
		curDoorAnim = StartCoroutine(DoorAnimation(Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f))));
		//transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0f));
		
		if(notifyOpen != null)
			notifyOpen();
	}

	[PunRPC]
	void CloseDoor() {
		//DO animation
		//transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0f));
		doorIsOpen = false;

		if(curDoorAnim!= null) {
			StopCoroutine(curDoorAnim);
		}
		curDoorAnim = StartCoroutine(DoorAnimation(Quaternion.Euler(new Vector3(0.0f, 90.0f, 0f))));

		if(notifyClose != null)
			notifyClose();
	}


	
	IEnumerator DoorAnimation(Quaternion target) {
		
		Quaternion startingRotation = transform.rotation; // have a startingRotation as well
		Quaternion targetRotation =  target;

		float time = 2f;
		float elapsedTime = 0;
		
		while (elapsedTime < time) {
			elapsedTime += Time.deltaTime; // <- move elapsedTime increment here
			
			transform.rotation = Quaternion.Slerp(startingRotation, targetRotation,  (elapsedTime / time)  );
			yield return new WaitForEndOfFrame ();
		}

		yield return null;
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
	

	//Debug: host open door
	void Update() {
		if(Input.GetKeyDown(KeyCode.H)) {
			OperateDoor();
		}
	}
}
