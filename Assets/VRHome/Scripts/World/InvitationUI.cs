using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvitationUI : MonoBehaviour {

	public GameObject invitationPanel;

	public int inv_from;
	public int inv_to;
	public string msg;

	public void OnGoButtonClicked(){ //ACC
		EntryExitManager.instance.OnSetUpMethod(inv_to, inv_from, inv_to);
		//???
		invitationPanel.SetActive(false);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			invitationPanel.SetActive(!invitationPanel.activeSelf);
		}
	}

	public void ReceiveInvitation(int from_player, int to_player, string msg) {

		inv_from = from_player;
		inv_to = to_player;
		this.msg = msg;

		invitationPanel.SetActive(true);
	}
}
