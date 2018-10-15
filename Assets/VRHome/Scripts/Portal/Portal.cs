using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.SportShooting;
public class Portal : MonoBehaviour {

	public string roomID = "";
	public PhotonView photonView;
	public GameObject entity;

	public bool visible;

	private BoxCollider col;

	void Awake() {
		visible = entity.activeSelf;
		col = GetComponent<BoxCollider>();
	}
	

	void SetToGoID(string id) {
		roomID = id;
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "MainCamera") {
			if(!roomID.Equals(""))
				GameController.Instance.JoinRoom(roomID);
		}
	}

	public void ShowEntity() {
		photonView.RPC("SetActiveByRPC", PhotonTargets.All, true);
	}

	public void DisappearEntity() {
		photonView.RPC("SetActiveByRPC", PhotonTargets.All, false);
	}

	[PunRPC]
	void SetActiveByRPC(bool active) {
		Debug.Log("Set active to " + active);
		entity.SetActive(active);

		if(PhotonNetwork.isMasterClient) {
			col.enabled = false;
		}else{
			col.enabled = active;
		}


		visible = entity.activeSelf;
	}


}
