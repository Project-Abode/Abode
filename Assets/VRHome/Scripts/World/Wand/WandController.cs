using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {

	int state = 0;

	Follower follower;
	public WandParticleController wandParticleController;

	public GameObject shadowPrefab;
	ShadowParticleController shadow;

	MovingDetector movingDetector;

	public delegate void RequestTransport();
	public RequestTransport requestTransport;

	// Vector3 hostBase;
	// Vector3 guestBase;

	public void Init(Transform player, Vector3 hostBase, Vector3 guestBase) {

		follower = GetComponent<Follower>();
		
		movingDetector = GetComponent<MovingDetector>();
		movingDetector.enabled = false;

		state = 0;

		var cameraTransform = player.GetChild(0).GetChild(2);
		follower.Init(cameraTransform);

		var shadowGO = PhotonNetwork.Instantiate(shadowPrefab.name, new Vector3(0,0,0), Quaternion.identity, 0) as GameObject;
		shadow = shadowGO.GetComponent<ShadowParticleController>();
		shadow.Init(transform, guestBase, hostBase);

		wandParticleController.PlayParticleEffect(0);
	}
	
	void OnGrabbed() {
		if(state != 0) return;
		
		follower.Disable();
		movingDetector.enabled = true;
		state = 1;
	}

	void OnMovementAchieved(){
		if(state != 1) return;
		
		StartCoroutine(TransportEffect());
		
		state = 2;
	}

	IEnumerator TransportEffect() {
		wandParticleController.PlayParticleEffect(2);
		shadow.PlayNetworkParticle(2);
		yield return new WaitForSeconds(5f);
		StartTransport();
		yield return null;
	}

	void StartTransport(){
		state = 3;
		wandParticleController.StopAll();
		shadow.StopAllNetworkParticle();

		if(requestTransport!=null)
			requestTransport();
		
		CleanUpWand();
	}

	void CleanUpWand() {
		PhotonNetwork.Destroy(shadow.gameObject);
		Destroy(gameObject);
	}


	bool lastIsMoving = false;
	void Update() {
		if(state == 1) {

			bool curIsMoving = movingDetector.IsMoving();

			if(lastIsMoving == false && curIsMoving == true) {
				wandParticleController.PlayParticleEffect(1);
				shadow.PlayNetworkParticle(1);
			}

			if(lastIsMoving == true && curIsMoving == false) {
				wandParticleController.StopAll();
				shadow.StopAllNetworkParticle();
			}

			lastIsMoving = curIsMoving;

			if(movingDetector.DoesMovingLastSeconds(3f)) {
				OnMovementAchieved();
			}
		}

	}


}
