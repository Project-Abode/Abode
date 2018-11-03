using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExvitationPromptController : MonoBehaviour {


	private AudioSource audioSource;

	private PhotonView photonView;
	
	public AudioClip clockSound;

	public GameObject rain;

	void Awake () {
		audioSource = GetComponent<AudioSource>();
		photonView = GetComponent<PhotonView>();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)) {
			photonView.RPC("PlayClockSound", PhotonTargets.All);
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			photonView.RPC("PlayRainEffect", PhotonTargets.All);
		}
	}

	[PunRPC]
	public void PlayClockSound() {
		audioSource.PlayOneShot(clockSound);
	}

	[PunRPC]
	public void PlayRainEffect() {
		rain.SetActive(true);
	}


}
