using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour {
	public Text msgtxt;
	public int forPlayer;
	//public Portal portal;
	//public GameObject portalBtn;
	
	//TODO:TV audio

	void Awake () {
		if(Settings.instance.id != forPlayer) {
			gameObject.SetActive(false);
		}
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.I)) {
			OnInviteClicked();
		}
		

		if(Input.GetKeyDown(KeyCode.P)) {
			OnPortalClicked();
		}

		// //HACK: need to add to event listener to guest join in
		// if(PhotonNetwork.playerList.Length >= 2) {
		// 	portalBtn.SetActive(true);
		// }else {
		// 	portalBtn.SetActive(false);
		// }

	}

	//Buttons:
	public void OnInviteClicked() {
		Debug.Log("Invite Clicked");
		SetMsg("Invitation sent. Awaiting for guest response...");
		MessageSystem.instance.SendInvitation(1,"Hi");
		
	}

	public void OnQuestionClicked() {
		Debug.Log("Question Clicked");
		
	}

	public void OnPortalClicked() {
		Debug.Log("Portal Clicked");

		switch(Settings.instance.method) {
			case 0:
				SetMsg("Guest's portal to home is outside the door.");
				break;
			case 1:
				SetMsg("Guest's magic wand is here to take him back home");
				break;
			
		}
		
		EntryExitManager.instance.OnSetUpMethod(0,1,1);

	}


	//[RPC]
	public void SetMsg(string msg) {
		msgtxt.text = msg;
	}


	//Callback
	public void OnGuestAccept() {
		SetMsg("Invatation accepted. Guest is on the way!");
		
	}

	public void OnGuestArrive() {
		SetMsg("Your Guest is Arrived!");
	}

	IEnumerator CleanMsg(float timer) {
		yield return new WaitForSeconds(timer);
		SetMsg("");
		yield return null;
	}


}
