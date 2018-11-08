using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayDoor : MonoBehaviour {

	//public int mirrorID;

	public delegate void NotifyTouched();
	public NotifyTouched notifyTouched;

	public void InitHallwayDoor(int id) {
		//mirrorID = id;
	}


	void OnTriggerEnter(Collider col) {

		if(col.gameObject.tag != "Hand") return;
		
		if(notifyTouched!=null)
			notifyTouched();

	}

	public void CleanDoor() {
		notifyTouched = null;
		//mirrorID = -1;
	}


}
