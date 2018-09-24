using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour {

    public float mouseSensitivity = 4.0f;
    public GameObject eyeCam;

    private const float MIN_VERT_ROTATION = -90;
    private const float MAX_VERT_ROTATION = 90;
    private const float CAMERA_CHECK_TIME = 0.1f;

    private Camera cam;
    private float horizRotation = 0;
    private float vertRotation = 0;

    private void Start()
    {
        StartCoroutine(CameraCheck());
	}
	
    private IEnumerator CameraCheck()
    {
        yield return new WaitForSeconds(CAMERA_CHECK_TIME);
        cam = gameObject.GetComponent<Camera>();
		if (VRFailed())
        {
            TakeControl();
        }
        else
        {
            NoOverride();
        }
    }

    private bool VRFailed()
    {
        //GameObject camObj = Camera.main.gameObject;
        SteamVR_Camera camScript = eyeCam.GetComponent<SteamVR_Camera>();
        return !camScript.enabled;
    }

    private void TakeControl()
    {
        print("No VR detected- mouse control override");
        cam.tag = "MainCamera";
        while (Camera.main != cam)
        {
            Camera.main.enabled = false;
        }
    }

    private void NoOverride()
    {
        print("VR detected- no override necessary");
        Destroy(gameObject);
    }

    private void Update()
    {
        float horizTurn = mouseSensitivity * Input.GetAxis("Mouse X");
        float vertTurn = mouseSensitivity * -Input.GetAxis("Mouse Y");
        horizRotation += horizTurn;
        vertRotation += vertTurn;
        vertRotation = Mathf.Clamp(vertRotation, MIN_VERT_ROTATION, MAX_VERT_ROTATION);
        transform.eulerAngles = new Vector3(vertRotation, horizRotation, 0);

        if (Input.GetKeyDown(KeyCode.X))
        {
            //this is weird and should only be used for debugging the meditation standing up part
            Vector3 parentPos = transform.parent.position;
            float newY = 1.5f - parentPos.y; //switch between 0 (sitting) and 1.5 (standing)
            Vector3 newPos = new Vector3(parentPos.x, newY, parentPos.z);
            transform.parent.position = newPos;
        }
    }
}
