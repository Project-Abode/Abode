using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftController : MonoBehaviour {

	FloatingObj floating;
	SimpleGrabable grabable;

	void Start () {
		grabable = GetComponent<SimpleGrabable>();
		//grabable.onGrab += OnGiftGrabbed;

		floating = GetComponent<FloatingObj>();
		
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Hand") {
			StopFloating();
		}
	}
	
	// void OnGiftGrabbed() {
	// 	StopFloating();
	// 	grabable.onGrab -= OnGiftGrabbed;
	// }

	public void StopFloating() {
		floating.enabled = false;
	}

	public void StartFloating() {
		floating.enabled = true;
	}


}
