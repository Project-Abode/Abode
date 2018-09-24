using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandInteraction : MonoBehaviour {

    private SteamVR_Controller.Device device;
    public SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Awake () {
        Debug.Log(trackedObj.transform.parent.gameObject.activeSelf);
        if(trackedObj.transform.parent.gameObject.activeSelf)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);
        }
    }
	
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger haptic pulse");
        if(device!=null)
        {
            device.TriggerHapticPulse(500);
        }
        
    }

}
