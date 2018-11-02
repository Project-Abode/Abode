using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEETrigger : EETrigger {

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag != "MainCamera") return;
		TriggerTeleport();
	}
}
