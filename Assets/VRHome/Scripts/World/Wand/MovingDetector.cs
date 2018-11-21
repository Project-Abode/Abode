using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDetector : MonoBehaviour {

	float movingLastingSeconds = 0f;
	Vector3 lastPosition;
	public double threshold = 0.1f;
	void Start () {
		lastPosition = transform.position;
	}

	void Update () {
		var distance = Vector3.Distance(transform.position,lastPosition);
		if(distance > threshold) {
			movingLastingSeconds += Time.deltaTime;
		}else {
			movingLastingSeconds = 0f;
		}
		lastPosition = transform.position;
	}


	public bool DoesMovingLastSeconds(float seconds) {
		if(movingLastingSeconds > seconds) {
			return true;
		}else {
			return false;
		}	
	}

	public bool IsMoving() {
		return (movingLastingSeconds > 0);
	}


}
