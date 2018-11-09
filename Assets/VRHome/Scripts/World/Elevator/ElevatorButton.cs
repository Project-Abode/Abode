using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour {

	public delegate void NotifyTouched();
	public NotifyTouched notifyTouched;

	void OnTriggerEnter(Collider col) {

		if(col.gameObject.tag != "Hand") return;
		
		if(notifyTouched!=null)
			notifyTouched();

	}


}
