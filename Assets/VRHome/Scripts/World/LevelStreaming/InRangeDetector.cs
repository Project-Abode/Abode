using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeDetector : MonoBehaviour {

	public string TargetTag = ""; 
	//MainCamera
	public bool inRange = false;

	public delegate void NotifyEnterArea();
	public NotifyEnterArea notifyEnterArea;
	

	void OnTriggerEnter(Collider col) {

		Debug.Log(gameObject.name + " Enter: " + col.gameObject.tag );

		if(col.gameObject.tag.Equals(TargetTag)) {
			inRange = true;
			
			if(notifyEnterArea!=null)
				notifyEnterArea();
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
