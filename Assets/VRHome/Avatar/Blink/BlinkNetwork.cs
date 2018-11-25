using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;

public class BlinkNetwork : MonoBehaviour {
	PhotonView photonView;
	BlinkController blinkController;
	void Start () {

		blinkController = GetComponentInChildren<BlinkController>();
		
		if(blinkController == null) {
			this.enabled = false;
		}
		
		photonView = GetComponent<PhotonView>();
	}


	void Update() {
		// if(Input.GetKeyDown(KeyCode.D)) {
		// 	NetWorkFadeOut();
		// }

		// if(Input.GetKeyDown(KeyCode.A)) {
		// 	NetWorkFadeIn();
		// }
	}
	

	public void NetWorkSetAvatarChoice(int avatar_choice){
		photonView.RPC("SetAvatarChoice", PhotonTargets.All, avatar_choice);
	}
	public void NetWorkFadeIn() {
		photonView.RPC("FadeIn", PhotonTargets.All);
	}

	public void NetWorkFadeOut() {
		photonView.RPC("FadeOut", PhotonTargets.All);
	}

	[PunRPC] 
	void SetAvatarChoice(int avatar_choice) {
		blinkController.avatar_choice = avatar_choice;
	}

	[PunRPC]
	void FadeIn() {
		blinkController.FadeIn();
	}

	[PunRPC]
	void FadeOut() {
		blinkController.FadeOut();
	}


}
