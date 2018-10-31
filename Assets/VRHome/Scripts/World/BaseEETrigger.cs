using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEETrigger : EETrigger {

	void Update () {

		if(Input.GetKeyDown(KeyCode.T)) {
			TriggerTeleport();
		}

	}
}
