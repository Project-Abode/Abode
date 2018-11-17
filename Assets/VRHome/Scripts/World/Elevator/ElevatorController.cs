using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

	public bool isDoorOpen = false;

	private PhotonView photonView;

	public GameObject doorObject;

	public ElevatorButton button;

	private AudioSource audioSource;
	//public List<AudioClip> clips;


	void Awake() {
		audioSource = GetComponent<AudioSource>();
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

	Coroutine doorCoroutine;
	float doorOPTime = 1.855f;

	[PunRPC]
	void CloseDoorRPC() {
		//doorObject.SetActive(true);
		
		isDoorOpen = false;

		if(doorCoroutine!=null) {
			audioSource.Stop();
			StopCoroutine(doorCoroutine);
		}
		
		doorCoroutine = StartCoroutine(DoorCoroutine(-1,doorOPTime));
;

		
	}

	IEnumerator DoorCoroutine(int dir, float time){

		audioSource.Play();
		Vector3 target = doorObject.transform.position + new Vector3(0,0,dir);
		Vector3 start = doorObject.transform.position;
		float elapsedTime = 0f;
      
 
      while (elapsedTime < time)
      {
		doorObject.transform.position = Vector3.Lerp(start, target, (elapsedTime / time));
          elapsedTime += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

		yield return null;
	}


	public void OpenDoor() {
		photonView.RPC("OpenDoorRPC", PhotonTargets.All);
	}

	[PunRPC]
	void OpenDoorRPC() {
		//doorObject.SetActive(false);

		isDoorOpen = true;
		
		if(doorCoroutine!=null) {
			audioSource.Stop();
			StopCoroutine(doorCoroutine);
		}
		
		doorCoroutine = StartCoroutine(DoorCoroutine(1,doorOPTime));
		
	}

}
