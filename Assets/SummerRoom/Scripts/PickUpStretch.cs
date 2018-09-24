using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStretch : MonoBehaviour
{
    private const int NONE = -1;
    private const int NUM_CONTROLLERS = 2;

    public static GameObject[] grabbableObjects = new GameObject[NUM_CONTROLLERS];

    public bool canStretch = true;
    public bool makeKinematicWhenReset = false;

    public bool specialOutline = false; //use special crystal sphere outline behavior

    public Shader highlightShader;
    public Color heldColor;
    public Color hoverColor;
    public Color stretchColor;
    public Color stretchHoverColor;

    public GameObject splashPrefab;

    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];

    private bool[] controllersInside = new bool[NUM_CONTROLLERS];

    public int holder = NONE;
    private int stretcher = NONE;

    private Holder insidePickupHolder = null;
    private bool onString = false;

    private Rigidbody rb;
    private Shader oldShader;
    private Renderer rend;

    private Vector3 startPos;
    private Quaternion startRotation;
    private Vector3 startScale;
    private Vector3 baseScale;
    private float stretchScale;

    private Renderer baseRend; //only used if specialOutline is true
    private Renderer outlineRend; //only used if specialOutline is true

    protected virtual void Start()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        if (specialOutline)
        {
            baseRend = transform.Find("Base").GetComponent<Renderer>();
            oldShader = baseRend.material.shader;
            outlineRend = transform.Find("Outline").GetComponent<Renderer>();
        }
        else
        {
            oldShader = rend.material.shader;
        }
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

        if (stretcher != NONE && canStretch)
        {
            Scale();
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
                if (CanStretch(controllerIndex))
                {
                    GrabStretch(controllerIndex);
                }
            }
            if (input.GetHairTriggerUp())
            {
                Release(controllerIndex);
            }

            if (input.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                ResetPos();
            }
        }
        catch (IndexOutOfRangeException)
        {
            //can't talk to controller, don't do anything
        }
    }

    private bool CanGrab(int controllerIndex)
    {
        return controllersInside[controllerIndex] && grabbableObjects[controllerIndex] == gameObject && holder == NONE;
    }

    private bool CanStretch(int controllerIndex)
    {
        return controllersInside[controllerIndex] && grabbableObjects[controllerIndex] == gameObject && holder != NONE && holder != controllerIndex && stretcher == NONE;
    }

    public void Grab(int controllerIndex)
    {
        SteamVR_TrackedObject controller = controllers[controllerIndex];
        holder = controllerIndex;
        AttachToController(controller);
        SetColor(heldColor);
        Rumble(controllerIndex);
    }

    protected virtual void AttachToController(SteamVR_TrackedObject controller)
    {
        gameObject.transform.parent = controller.gameObject.transform;
        rb.isKinematic = true;
    }

    private void GrabStretch(int controllerIndex)
    {
        stretcher = controllerIndex;
        baseScale = transform.localScale;
        stretchScale = ControllerDistance();
        SetColor(stretchColor);
        Rumble(controllerIndex);
    }

    public void Release(int controllerIndex)
    {
        if (stretcher == controllerIndex)
        {
            grabbableObjects[controllerIndex] = null;
            stretcher = NONE;
            Rumble(controllerIndex);
        }

        if (holder == controllerIndex)
        {
            grabbableObjects[controllerIndex] = null;
            holder = NONE;
            ReleaseFromController(controllers[controllerIndex]);
            Rumble(controllerIndex);

            if (stretcher != NONE)
            {
                //transfer to stretcher
                holder = stretcher;
                stretcher = NONE;
                SetColor(heldColor);
                AttachToController(controllers[holder]);
            }

            if (insidePickupHolder != null && insidePickupHolder.CanApply())
            {
                insidePickupHolder.Apply(gameObject);
                SetNotGrabbable(controllerIndex);
                gameObject.SetActive(false);
                return;
            }
        }

        if (controllersInside[controllerIndex])
        {
            SetGrabbable(controllerIndex);
        }
        else
        {
            SetNotGrabbable(controllerIndex);
        }
    }

    protected virtual void ReleaseFromController(SteamVR_TrackedObject controller)
    {
        gameObject.transform.parent = null;
        if (!onString)
        {
            rb.isKinematic = false;
        }
        SteamVR_Controller.Device input = SteamVR_Controller.Input((int)controller.index);
        rb.velocity = input.velocity;
        rb.angularVelocity = input.angularVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Holder h = other.GetComponent<Holder>();
        if (h != null)
        {
            insidePickupHolder = h;
        }

        if (other.CompareTag("String"))
        {
            onString = true;
        }

        if (other.CompareTag("SplashZone"))
        {
            Instantiate(splashPrefab, transform.position, Quaternion.identity);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (controllers[controllerIndex] != null && other.gameObject == controllers[controllerIndex].gameObject)
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

        if (stretcher == NONE && holder != NONE)
        {
            SetShader(highlightShader);
            SetColor(heldColor);
        }

        Holder h = other.GetComponent<Holder>();
        if (h == insidePickupHolder)
        {
            insidePickupHolder = null;
        }

        if (other.CompareTag("String"))
        {
            onString = false;
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
            SetShader(highlightShader);
            SetColor(hoverColor);
        }
        if (CanStretch(controllerIndex))
        {
            SetShader(highlightShader);
            SetColor(stretchHoverColor);
        }
    }

    public void SetNotGrabbable(int controllerIndex)
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
            SetShader(oldShader);
        }
    }

    private void SetShader(Shader shader)
    {
        if (specialOutline)
        {
            baseRend.material.shader = shader;
            bool isOutlined = shader != oldShader;
            outlineRend.gameObject.SetActive(isOutlined);
        }
        else
        {
            rend.material.shader = shader;
        }
    }

    private void SetColor(Color color)
    {
        if (specialOutline)
        {
            baseRend.material.SetColor("_OutlineColor", color);
            outlineRend.material.SetColor("_Color", color);
        }
        else
        {
            rend.material.SetColor("_OutlineColor", color);
        }
    }

    private void ResetPos()
    {
        if (stretcher != NONE)
        {
            Release(stretcher);
        }

        if (holder != NONE)
        {
            Release(holder);
        }

        transform.position = startPos;
        transform.rotation = startRotation;
        transform.localScale = startScale;
        rb.velocity = Vector3.zero;

        if (makeKinematicWhenReset)
        {
            rb.isKinematic = true;
        }
    }

    private void Rumble(int controllerIndex)
    {
        SteamVR_Controller.Device input = GetInput(controllerIndex);
        input.TriggerHapticPulse(1200);
    }

    private float ControllerDistance()
    {
        Vector3 holderPos = controllers[holder].transform.position;
        Vector3 stretcherPos = controllers[stretcher].transform.position;
        return Vector3.Distance(holderPos, stretcherPos);
    }

    private void Scale()
    {
        float distance = ControllerDistance();
        float scaled = distance / stretchScale;
        transform.localScale = baseScale * scaled;
    }
}
