using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExvitationPromptController : MonoBehaviour {
	private AudioSource audioSource;
	private PhotonView photonView;
	
	public AudioClip clockSound;
	public GameObject rain;
	public GameObject candle;


	void Awake () {
		audioSource = GetComponent<AudioSource>();
		photonView = GetComponent<PhotonView>();
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


	List<string> ExvitationDict = new List<string>() {
		"PlayClockSound",//"Clock",
        "PlayRainEffect",//"Rain", 
        "PlayCandle"//"Candle", 
        //"Gift Hearth",
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


}
