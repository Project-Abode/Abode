using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDetection : MonoBehaviour {
	public string targetTag = "";

	public delegate void NotifyTouched();
	public NotifyTouched notifyTouched;

	public bool isActive = false;

	void Awake() {
		isActive = false;
	}

	void OnTriggerEnter(Collider col) {
		if(!isActive) return;

		if(!col.gameObject.tag.Equals(targetTag)) return;
		
		if(notifyTouched!=null)
			notifyTouched();
	}	
	
	public void SetActive(bool val) {
		isActive = val;
	}

}
