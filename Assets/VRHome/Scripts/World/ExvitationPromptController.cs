using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExvitationPromptController : MonoBehaviour {
	private AudioSource audioSource;
	private PhotonView photonView;
	
	public AudioClip clockSound;
	public GameObject rain;
	public GameObject candle;

	public Transform GiftSpawnPoint;


	void Awake () {
		audioSource = GetComponent<AudioSource>();
		photonView = GetComponent<PhotonView>();

		if(Settings.instance.excontrol == 2) // timer
			EntryExitManager.instance.notifyGuestArrive += StartTimer;

	}
	
	void Update () {
		// if(Input.GetKeyDown(KeyCode.C)) {
		// 	photonView.RPC("PlayClockSound", PhotonTargets.All);
		// }

		// if(Input.GetKeyDown(KeyCode.R)) {
		// 	photonView.RPC("PlayRainEffect", PhotonTargets.All);
		// }

		// if(Input.GetKeyDown(KeyCode.K)) {
		// 	photonView.RPC("PlayCandle", PhotonTargets.All);
		// }

		if(Input.GetKeyDown(KeyCode.Space)) {
			PlayExvitation();
		}

	}

	void OnGuestArrive() {
		if(Settings.instance.exvitation == 2) {
			photonView.RPC(ExvitationDict[2], PhotonTargets.All);
		}
	}


	List<string> ExvitationDict = new List<string>() {
		"PlayClockSound",//"Clock",
        "PlayRainEffect",//"Rain", 
        "PlayCandle",//"Candle", 
        "ShowGift"
        //"Gift Garden", 
        //"Air Balloon"
	};

	public void PlayExvitation() {

		

		var exvitationIndex = Settings.instance.exvitation;
		if(exvitationIndex >= ExvitationDict.Count) {
			Debug.Log("Exvitaton out of index");
			return;
		}

		photonView.RPC(ExvitationDict[exvitationIndex], PhotonTargets.All);
	}

	[PunRPC]
	public void PlayClockSound() {
		if(audioSource!=null && clockSound!=null)
			audioSource.PlayOneShot(clockSound);
	}

	[PunRPC]
	public void PlayRainEffect() {
		if(rain!=null)
			rain.SetActive(true);
	}

	[PunRPC]
	public void PlayCandle() {
		if(candle!=null)
			candle.SetActive(true);
	}


	[PunRPC]
	public void ShowGift() {
		if(Settings.instance.id == 1) // is guest
		{
			var gift  = PhotonNetwork.Instantiate("Gift", GiftSpawnPoint.position, Quaternion.identity, 0) as GameObject;
			gift.tag = "grabable";
			var giftController = gift.GetComponent<GiftController>();
			if(giftController!=null) {
				giftController.StartFloating();
			}
		}

	}

	public void StartTimer() {
		
		if(Settings.instance.exvitation == 2) // candle
		{
			//TODO: set candle duration
			PlayExvitation();
			return;
		}


		var time = Settings.instance.timer * 60;
		StartCoroutine(TimerCountDownForExvitation(time));
	}


	IEnumerator TimerCountDownForExvitation(float time) {
		yield return new WaitForSeconds(time);
		PlayExvitation();
	}


}
