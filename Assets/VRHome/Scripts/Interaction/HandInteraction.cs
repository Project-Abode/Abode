using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandInteraction : MonoBehaviour {

    private SteamVR_Controller.Device device;
    public SteamVR_TrackedObject trackedObj;

    bool activated = false;

    private void Update()
    {
        if (!activated)
        {
            if(trackedObj.transform.parent.gameObject.activeSelf)
            {
                Debug.Log("Controller activated");
                activated = true;
                device = SteamVR_Controller.Input((int)trackedObj.index);
            }
        }
    }

    float pulseDuration = 0.1f;

    Coroutine coroutine;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger haptic pulse");
        if(device!=null)
        {
            Debug.Log("device not null");
            //device.TriggerHapticPulse(pulseDuration,EVRButtonId.k_EButton_Grip);
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(LongVibration(pulseDuration, 0.5f));
        }
        
    }

    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }

}
