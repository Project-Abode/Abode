using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{

    private const int NUM_CONTROLLERS = 2;

    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];
    private bool[] controllersInside = new bool[NUM_CONTROLLERS];

    protected GameObject heldObject = null;

    protected virtual void Start()
    {
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
    }

    private void OnTriggerStay(Collider other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (other.gameObject == controllers[controllerIndex].gameObject)
            {
                controllersInside[controllerIndex] = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (other.gameObject == controllers[controllerIndex].gameObject)
            {
                controllersInside[controllerIndex] = false;
            }
        }
    }

    protected virtual void Update()
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            CheckController(controllerIndex);
        }
    }

    private void CheckController(int controllerIndex)
    {
        try
        {
            SteamVR_Controller.Device input = GetInput(controllerIndex);
            if (input.GetHairTriggerDown())
            {
                if (CanGrab(controllerIndex))
                {
                    Grab(controllerIndex);
                }
            }

            if (input.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                Reset();
            }
        }
        catch (IndexOutOfRangeException)
        {
            //can't talk to controller, don't do anything
        }
    }

    private SteamVR_Controller.Device GetInput(int controllerIndex)
    {
        SteamVR_TrackedObject controller = controllers[controllerIndex];
        return SteamVR_Controller.Input((int)controller.index);
    }

    private bool CanGrab(int controllerIndex)
    {
        return controllersInside[controllerIndex] && heldObject != null;
    }

    private void Grab(int controllerIndex)
    {
        heldObject.SetActive(true);
        heldObject.transform.position = controllers[controllerIndex].gameObject.GetComponent<SphereCollider>().center + controllers[controllerIndex].gameObject.transform.position;
        PickUpStretch pickUp = heldObject.GetComponent<PickUpStretch>();
        if (pickUp != null)
        {
            pickUp.Grab(controllerIndex);
        }
        Remove();
    }

    private void Reset()
    {
        //TODO
        //Remove();
    }

    public bool CanApply()
    {
        return heldObject == null;
    }

    public virtual void Apply(GameObject obj)
    {
        heldObject = obj;
    }

    protected virtual void Remove()
    {
        heldObject = null;
    }
}
