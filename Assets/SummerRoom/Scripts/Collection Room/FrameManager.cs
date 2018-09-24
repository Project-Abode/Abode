using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FrameManager : MonoBehaviour {

    private static List<ClothSphereColliderPair> canvasSphereColliders;
    private static List<ClothSphereColliderPair> canvasCapsuleColliders;
    private VideoPlayer vp;

    public GameObject myCanvas;
    public GameObject backTrigger;
    GameObject heldMedia;
    Material myMaterial;

    private float transitionTime = 0.5f;
    private const int NUM_CONTROLLERS = 2;
    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];
    private bool[] controllersBehindCanvas = new bool[NUM_CONTROLLERS];

	// Use this for initialization
	void Start () {
        myMaterial = myCanvas.GetComponent<Renderer>().material;
        SetCanvasScale();
        vp = myCanvas.GetComponent<VideoPlayer>();
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
        controllersBehindCanvas[0] = false;
        controllersBehindCanvas[1] = false;
        canvasSphereColliders = new List<ClothSphereColliderPair>();
        canvasCapsuleColliders = new List<ClothSphereColliderPair>();
        canvasSphereColliders.Add(new ClothSphereColliderPair(controllers[0].GetComponent<SphereCollider>()));
        canvasSphereColliders.Add(new ClothSphereColliderPair(controllers[1].GetComponent<SphereCollider>()));
        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            try {
                SteamVR_TrackedObject controller = controllers[controllerIndex];
                if (controller.gameObject.activeSelf){
                    SteamVR_Controller.Device input = SteamVR_Controller.Input((int)controller.index);
                    if (input.GetHairTriggerDown())
                    {
                        /*canvasSphereColliders.Add(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();*/
                        if (controllersBehindCanvas[controllerIndex]){
                            removeImage(controllerIndex);
                        }
                    }
                    if (input.GetHairTriggerUp())
                    {
                        /*canvasSphereColliders.Remove(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();*/
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                //can't talk to controller, don't do anything
            }
        }
	}

    public void setImage(GameObject obj) {
        Transform t = obj.transform.Find("Quad");
        if (t != null && heldMedia == null)
        {
            GameObject image = t.gameObject;
            VideoPlayer vid = image.GetComponent<VideoPlayer>();
            if (vid != null)
            {
                //use video texture
                StartCoroutine(TransitionToDisplay(null, vid.clip));
            }
            else
            {
                //use image texture
                StartCoroutine(TransitionToDisplay(image.GetComponent<Renderer>().material.mainTexture, null));

            }
            heldMedia = obj;
            PickUpStretch pickerUpper = heldMedia.GetComponent<PickUpStretch>();
            pickerUpper.SetNotGrabbable(pickerUpper.holder);
            heldMedia.SetActive(false);
        }
    }

    public void mayHaveFoundController(GameObject other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (controllers[controllerIndex] != null && other == controllers[controllerIndex].gameObject)
            {
                controllersBehindCanvas[controllerIndex] = true; 
            }
       }
   }

    public void mayHaveLostController(GameObject other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (controllers[controllerIndex] != null && other == controllers[controllerIndex].gameObject)
            {
                controllersBehindCanvas[controllerIndex] = false;
            }
         }
    } 

    public void removeImage(int controllerIndex)
    {
        StartCoroutine(TransitionToDefault());
        StartCoroutine(DisableBack());
        heldMedia.SetActive(true);
        heldMedia.transform.position = (2* controllers[controllerIndex].gameObject.GetComponent<SphereCollider>().center) + controllers[controllerIndex].gameObject.transform.position;
        heldMedia.GetComponent<PickUpStretch>().Grab(controllerIndex);
        heldMedia = null;
    }

    private IEnumerator TransitionToDefault()
    {
        float oldValue = myMaterial.GetFloat("_Threshold");
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            myMaterial.SetFloat("_Threshold", Mathf.Lerp(oldValue, 0.0f, t / transitionTime));
            yield return new WaitForEndOfFrame();
        }
        myMaterial.SetFloat("_Threshold", 0.0f);
        vp.Stop();
        vp.clip = null;
    }

    private IEnumerator TransitionToDisplay(Texture newTex, VideoClip newVid)
    {
        
        SetCanvasScale();
        if (newVid != null){
            vp.clip = newVid;
            vp.Prepare();
            while (!vp.isPrepared)
            {
                yield return null;
            }
            vp.Play();
        } else {
            myMaterial.SetTexture("_DisplayTex", newTex);
        }
        float oldValue = myMaterial.GetFloat("_Threshold");
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            myMaterial.SetFloat("_Threshold", Mathf.Lerp(oldValue, 1.0f, t / transitionTime));
            yield return new WaitForEndOfFrame();
        }
        myMaterial.SetFloat("_Threshold", 1.0f);
    }

    private void SetCanvasScale()
    {
        myMaterial.SetFloat("_ScaleWidth", myCanvas.transform.lossyScale.x);
        myMaterial.SetFloat("_ScaleHeight", myCanvas.transform.lossyScale.y);
    }

    private IEnumerator DisableBack()
    {
        backTrigger.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        backTrigger.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        myMaterial.SetFloat("_Threshold", 0);
        myMaterial.SetFloat("_ScaleWidth", 1);
        myMaterial.SetFloat("_ScaleHeight", 1);
    }
}
