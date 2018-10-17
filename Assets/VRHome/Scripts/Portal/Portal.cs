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

	public float leaveDuration = 5f;
	void OnTriggerEnter(Collider col) {
		if(col.tag == "MainCamera") {
			if(!roomID.Equals("")) {

				//TODO: Count down and VFX change and sound
				PlayGoEffect();
				StartCoroutine(LeaveAfterSeconds(leaveDuration));
			}
				
		}
	}
	
	IEnumerator LeaveAfterSeconds(float duration) {
		yield return new WaitForSeconds(duration);
		GameController.Instance.JoinRoom(roomID);
		yield return null;
	}

	public void ShowEntity() {
		photonView.RPC("SetActiveByRPC", PhotonTargets.All, true);
	}

	public void DisappearEntity() {
		photonView.RPC("SetActiveByRPC", PhotonTargets.All, false);
	}

	public PortalEffectController _portalEffectController;

	[PunRPC]
	void PlayGoEffect() {
		var _audio =GetComponent<AudioSource>();
		if(_audio)
			_audio.Play();
		if(_portalEffectController) {
			_portalEffectController.PlayGoEffect(leaveDuration);
		}
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

	//Debug
	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			PlayGoEffect();
		}
	}


}
