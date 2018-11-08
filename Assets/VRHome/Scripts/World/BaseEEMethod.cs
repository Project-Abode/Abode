using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEEMethod : EEMethod {


    //public GameObject entity;
    private EETrigger trigger;
    public GameObject triggerGO;

    public Vector3 toGoPos;

    override public void InitMethod(Transform VRPlayer = null) {
        trigger = Instantiate(triggerGO).GetComponent<EETrigger>();
        if(trigger!=null)
            trigger.Init(this);

        if(to == 0) {
            toGoPos = new Vector3(0,0,0);
        }else {
            toGoPos = new Vector3(1000,1000,1000);
        }

	}

    override public void CleanUpMethod() {
        if(trigger!=null)
            trigger.DestroyThisTrigger();
        Destroy(gameObject);
    }

    override public void TeleportTriggered() {
		EntryExitManager.instance.TeleportPlayerTo(to, toGoPos);
	} 

	
}
