using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour {


	private PhotonView photonView;
	
	public List<InvitationUI> invitationControllers;


	public static MessageSystem instance = null;
	void Awake() {
		if (instance == null)
			 instance = this;
		else if (instance != this)
             Destroy(gameObject); 

		photonView = GetComponent<PhotonView>();
	}

	void Update () {
		
	}


	public void SendInvitation(int to_player, string message) {
		
		photonView.RPC("ReceiveInvitation", PhotonTargets.Others, Settings.instance.id, to_player, message);

	}

	public void SendAC(int to_player) {

	}


	[PunRPC]
	public void ReceiveInvitation(int from_player, int to_player, string msg) {
		if(to_player!=Settings.instance.id) return;

		invitationControllers[to_player].ReceiveInvitation(from_player, to_player, msg);

	}

	[PunRPC]
	public void ReceiveAC() {

	}



}
