using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunVoiceController : MonoBehaviour {

	private PhotonVoiceRecorder photonVoiceRecorder;

	void Awake() {
		photonVoiceRecorder= GetComponent<PhotonVoiceRecorder>();
	}
	
	void Update () {

		if(photonVoiceRecorder==null) return;

		var playerList = GameObject.FindGameObjectsWithTag("Player");

		if(playerList.Length >= 2) {
			double distance = Vector3.Distance(playerList[0].transform.position,playerList[1].transform.position);
			
			if(distance < 10) {
				photonVoiceRecorder.Transmit = true;
			}else {
				photonVoiceRecorder.Transmit = false;
			}

		}

	}
}
