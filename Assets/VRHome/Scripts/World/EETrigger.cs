using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EETrigger : MonoBehaviour {
	public EEMethod eeMethod;
	public void Init(EEMethod ee) {
		eeMethod = ee;
	}

	public void TriggerTeleport() {
		eeMethod.TeleportTriggered();
	}

	public void DestroyThisTrigger() {
		Destroy(gameObject);
	}

}

