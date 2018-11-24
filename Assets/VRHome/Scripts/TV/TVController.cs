using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour {
	public Text msgtxt;
	public int forPlayer;
	//public Portal portal;
	//public GameObject portalBtn;
	
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
		
		//if(socket) {
		//	socket.SendMyMessage("send invitation");
		//}

		MessageSystem.instance.SendInvitation(1,"Hi");
		
	}

	public void OnQuestionClicked() {
		Debug.Log("Question Clicked");
		
	}

	public void OnPortalClicked() {
		Debug.Log("Portal Clicked");

		SetMsg("Your portal to home is next to the door.");

		EntryExitManager.instance.OnSetUpMethod(0,1,1);

		// if(!portal.visible) {
		// 	portal.ShowEntity();
		// 	SetMsg("Your portal to home is next to the door.");
		// }else {
		// 	portal.DisappearEntity();
		// 	SetMsg("");
		// }
	}


	//[RPC]
	public void SetMsg(string msg) {
		msgtxt.text = msg;
	}


	//Callback
	public void GuestAccept() {
		SetMsg("Invatation accepted. Guest is on the way!");
	}

	public void GuestArrive() {
		SetMsg("");
	}

	public void GuestCancel() {
		
	}




}
