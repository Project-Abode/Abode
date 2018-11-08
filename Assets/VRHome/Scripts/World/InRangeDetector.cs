using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeDetector : MonoBehaviour {

	public string TargetTag = ""; 
	//MainCamera
	public bool inRange = false;

	void OnTriggerEnter(Collider col) {

		Debug.Log(gameObject.name + " Enter: " + col.gameObject.tag );

		if(col.gameObject.tag.Equals(TargetTag)) {
			inRange = true;
		}
	}

	void OnTriggerStay(Collider col) {
		if(col.gameObject.tag.Equals(TargetTag)){
			inRange = true;
		}
	}

	void OnTriggerExit(Collider col) {
		Debug.Log(gameObject.name + " Leave: " + col.gameObject.tag );
		if(col.gameObject.tag.Equals(TargetTag)) {
			inRange = false;
		}
	}

	public bool InRange() {
		return inRange;
	}

}
