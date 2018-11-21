using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowParticleController : WandParticleController {
	Transform target;
	Vector3 targetBase;
	Vector3 myBase;

	PhotonView photonView;

	bool inited = false;
	public void Init(Transform _target, Vector3 _targetBase, Vector3 _myBase) {
		target = _target;
		targetBase = _targetBase;
		myBase = _myBase;

		inited = true;
	}

	void Awake() {
		photonView = GetComponent<PhotonView>();
	}
	
	void Update () {
		if(inited) {
			transform.position = target.position - targetBase + myBase;
		}
	}

	public void PlayNetworkParticle(int index) {
		photonView.RPC("PlayNetworkParticleRPC", PhotonTargets.All, index);
	}

	[PunRPC]
	void PlayNetworkParticleRPC(int index) {
		PlayParticleEffect(index);
	}

	public void StopAllNetworkParticle() {
		photonView.RPC("StopAllNetworkParticleRPC", PhotonTargets.All);
	}

	[PunRPC]
	void StopAllNetworkParticleRPC() {
		StopAll();
	}

	
}
