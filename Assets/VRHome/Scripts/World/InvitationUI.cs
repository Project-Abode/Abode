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

		SendAcception(inv_from);
		invitationPanel.SetActive(false);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			invitationPanel.SetActive(!invitationPanel.activeSelf);
		}
	}

	public void ReceiveInvitation(int from_player, int to_player, string msg) {

		if(from_player < 0 || to_player < 0) return;

		inv_from = from_player;
		inv_to = to_player;
		this.msg = msg;

		invitationPanel.SetActive(true);
	}

	void SendAcception(int to_host) {
		if(to_host < 0) return;
		MessageSystem.instance.SendAC(to_host);
	}

}
