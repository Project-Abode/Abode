using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private const int NO_HOLDER = -1;
    private const int NUM_CONTROLLERS = 2;

    private static GameObject[] grabbableObjects = new GameObject[NUM_CONTROLLERS];

    public Shader highlightShader;
    public Color heldColor;
    public Color hoverColor;
    public GameObject splashPrefab;
    public Transform respawnPoint;

    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];

    private bool[] controllersInside = new bool[NUM_CONTROLLERS];

    private int holder = NO_HOLDER;

    private Rigidbody rb;
    private Shader oldShader;
    private Renderer rend;

    private Vector3 startPos;

    protected virtual void Start()
    {
        startPos = gameObject.transform.position;
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        oldShader = rend.material.shader;
    }

    private SteamVR_Controller.Device GetInput(int controllerIndex)
    {
        SteamVR_TrackedObject controller = controllers[controllerIndex];
        return SteamVR_Controller.Input((int)controller.index);
    }

    protected virtual void Update()
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            CheckController(controllerIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<Slide>() == null)
        {
            transform.position = new Vector3(3, 0, 0);
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
            if (input.GetHairTriggerUp())
            {
                Release(controllerIndex);
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

    private bool CanGrab(int controllerIndex)
    {
        return controllersInside[controllerIndex] && grabbableObjects[controllerIndex] == gameObject && holder == NO_HOLDER;
    }

    private void Grab(int controllerIndex)
    {
        SteamVR_TrackedObject controller = controllers[controllerIndex];
        holder = controllerIndex;
        AttachToController(controller);
        SetColor(heldColor);
        SteamVR_Controller.Device input = GetInput(controllerIndex);
        Rumble(input);

        //clear the other controller's grabbable object if it was this
        int otherControllerIndex = 1 - controllerIndex;
        if (grabbableObjects[otherControllerIndex] == gameObject)
        {
            grabbableObjects[otherControllerIndex] = null;
        }
    }

    protected virtual void AttachToController(SteamVR_TrackedObject controller)
    {
        gameObject.transform.parent = controller.gameObject.transform;
        rb.isKinematic = true;
    }

    private void Release(int controllerIndex)
    {
        if (holder == controllerIndex)
        {
            grabbableObjects[controllerIndex] = null;
            holder = NO_HOLDER;
            ReleaseFromController(controllers[controllerIndex]);
            SteamVR_Controller.Device input = GetInput(controllerIndex);
            Rumble(input);

            if (controllersInside[controllerIndex])
            {
                SetGrabbable(controllerIndex);
            }
            else
            {
                SetNotGrabbable(controllerIndex);
            }
        }
    }

    protected virtual void ReleaseFromController(SteamVR_TrackedObject controller)
    {
        gameObject.transform.parent = null;
        rb.isKinematic = false;
        SteamVR_Controller.Device input = SteamVR_Controller.Input((int)controller.index);
        rb.velocity = input.velocity;
        rb.angularVelocity = input.angularVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        ActionIfBowl(other, b => b.AddObject());

        if (other.gameObject.CompareTag("SplashZone"))
        {
            Instantiate(splashPrefab, transform.position, Quaternion.identity);
            Respawn();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (other.gameObject == controllers[controllerIndex].gameObject)
            {
                controllersInside[controllerIndex] = true;
                SetGrabbable(controllerIndex);
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
            if (holder != controllerIndex)
            {
                SetNotGrabbable(controllerIndex);
            }
        }

        ActionIfBowl(other, b => b.RemoveObject());
    }

    private void ActionIfBowl(Collider other, Action<Bowl> action)
    {
        Transform parent = other.gameObject.transform.parent;
        if (parent != null)
        {
            Bowl bowl = parent.gameObject.GetComponent<Bowl>();
            if (bowl != null)
            {
                action(bowl);
            }
        }
    }

    private void SetGrabbable(int controllerIndex)
    {
        if (grabbableObjects[controllerIndex] == null)
        {
            grabbableObjects[controllerIndex] = gameObject;
        }

        if (CanGrab(controllerIndex))
        {
            rend.material.shader = highlightShader;
            SetColor(hoverColor);
        }
    }

    private void SetNotGrabbable(int controllerIndex)
    {
        if (grabbableObjects[controllerIndex] == gameObject)
        {
            grabbableObjects[controllerIndex] = null;
        }

        bool noneInside = true;
        for (int ci = 0; ci < NUM_CONTROLLERS; ci++)
        {
            if (controllersInside[ci])
            {
                noneInside = false;
            }
        }
        if (noneInside)
        {
            rend.material.shader = oldShader;
        }
    }

    private void SetColor(Color color)
    {
        rend.material.SetColor("_OutlineColor", color);
    }

    private void Reset()
    {
        if (holder != NO_HOLDER)
        {
            Release(holder);
        }
        gameObject.transform.position = startPos;
        rb.velocity = Vector3.zero;
    }

    private void Rumble(SteamVR_Controller.Device input)
    {
        input.TriggerHapticPulse(1200);
    }

    private void Respawn()
    {
        if (holder != NO_HOLDER)
        {
            Release(holder);
        }
        gameObject.transform.position = respawnPoint.position;
        rb.velocity = Vector3.zero;
    }
}
