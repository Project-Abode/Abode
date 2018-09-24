using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallInterior : MonoBehaviour {

    private const int NUM_CONTROLLERS = 2;
    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];

    Material myMaterial;

	// Use this for initialization
	void Start () {
        myMaterial = GetComponent<Renderer>().material;
        myMaterial.mainTexture = PanoramicBall.viewedImage;
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            try {
                SteamVR_TrackedObject controller = controllers[controllerIndex];
                if (controller.gameObject.activeSelf){
                    SteamVR_Controller.Device input = SteamVR_Controller.Input((int)controller.index);
                    if (input.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                    {
                        SceneManager.LoadScene("Collection Room");
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                //can't talk to controller, don't do anything
            }
        }
	}
}
