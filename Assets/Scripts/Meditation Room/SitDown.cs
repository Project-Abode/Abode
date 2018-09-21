using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SitDown : MonoBehaviour {

    public string menuScene;
    public GameObject[] audioToFade;
    public ExitText exitText;

    private const float SIT_TIME = 1.0f; //time required in sitting position before overlay disappears, in seconds
    private const float SIT_HEIGHT = 1.0f; //maximum y-position above floor that counts as sitting, in meters
    private const float FADE_IN_TIME = 4.0f; //time to fade everything in after sitting
    private const float FADE_OUT_TIME = 4.0f; //time to fade everything out after standing
    private const float FADE_OUT_HOLD_TIME = 2.0f; //time after fadeout completes to load menu

    private Transform camTransform;
    private bool sittingInProgress = false;
    private bool isQuitting = false;
    private Renderer[] childRenderers;
    private List<GameObject> toReactivate;
    private bool done = false; //fade-in completed, user is sitting
    private GameObject startLight; //starting light attached to this game object

	private void Awake()
    {
        camTransform = Camera.main.transform;

        startLight = transform.Find("Start Light").gameObject;

        //Deactivate everything else except gameObject and [CameraRig], add it to a list to reactivate
        toReactivate = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = scene.GetRootGameObjects(); //UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in rootObjects)
        {
            if (ShouldDeactivate(obj))
            {
                obj.SetActive(false);
                toReactivate.Add(obj);
            }
        }

        int childCount = gameObject.transform.childCount;
        childRenderers = new Renderer[childCount - 1]; //Minus one for the light.
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.SetActive(true);
            if(child != startLight) childRenderers[i] = child.GetComponent<Renderer>();
        }
    }

    private bool ShouldDeactivate(GameObject obj)
    {
        //Returns whether this object should be hidden in the startup sequence
        return //obj.activeSelf &&
               obj != gameObject &&
               obj.name != "[CameraRig]" &&
               obj.name != "[SteamVR]" &&
               obj.name != "Meditation_Mat";
    }
    
    private void Update()
    {
        if (!done && !sittingInProgress && IsSitting())
        {
            sittingInProgress = true;
            StartCoroutine(WaitSitting());
            if (exitText != null) exitText.setStartTimer(true);
            addAudioTimer();
        }

        if (done && !isQuitting && !IsSitting())
        {
            isQuitting = true;
            //StartCoroutine(FadeOut());
        }
    }

    private bool IsSitting()
    {
        return camTransform.position.y <= 1;
    }

    private IEnumerator WaitSitting()
    {
        for (float t = 0; t < SIT_TIME; t += Time.deltaTime)
        {
            if (!IsSitting())
            {
                sittingInProgress = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        startLight.SetActive(false);

        foreach (GameObject obj in toReactivate)
        {
            obj.SetActive(true);
            /*//make it completely transparent
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                SetTransparency(rend, 0);
            }*/
        }

        SetText("THANK YOU.");

       //Fade everything in
        for (float t = 0; t < FADE_IN_TIME; t += Time.deltaTime)
        {
            SetEverythingTransparency(t / FADE_IN_TIME);
            yield return new WaitForEndOfFrame();
        }

        ClearWalls();
        done = true;
    }

    private void SetText(string text)
    {
        foreach (Renderer rend in childRenderers)
        {
            TextMesh textMesh = rend.gameObject.GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.text = text;
            }
        }
    }

    private void SetEverythingTransparency(float transparency)
    {
        /*//set everything (with a renderer)'s color alpha to transparency
        foreach (GameObject obj in toReactivate)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                SetTransparency(obj.GetComponent<Renderer>(), transparency);
            }
        }*/

        //Fade dark walls out
        float inverseTransparency = 1 - transparency;
        foreach (Renderer rend in childRenderers)
        {
            SetTransparency(rend, inverseTransparency);
        }
    }

    private void SetTransparency(Renderer rend, float transparency)
    {
        Color oldCol = rend.material.color;
        rend.material.color = new Color(oldCol.r, oldCol.g, oldCol.b, transparency);
    }

    private IEnumerator FadeOut()
    {
        foreach (Renderer rend in childRenderers)
        {
            rend.gameObject.SetActive(true);
        }
        SetText("NOW EXITING.");
        
        List<AudioSource> audioSrcs = new List<AudioSource>();
        List<float> audioVols = new List<float>();
        foreach (GameObject audioObj in audioToFade)
        {
            AudioSource[] objSrcs = audioObj.GetComponents<AudioSource>();
            foreach (AudioSource audioSrc in objSrcs)
            {
                audioSrcs.Add(audioSrc);
                audioVols.Add(audioSrc.volume);
            }
        }

        for (float t = 0; t < FADE_OUT_TIME; t += Time.deltaTime)
        {
            if (IsSitting())
            {
                isQuitting = false;
                ClearWalls();
                SetAllVolume(audioSrcs, audioVols, 0);
                yield break;
            }
            float tPercent = t / FADE_OUT_TIME;
            SetEverythingTransparency(1 - tPercent);
            SetAllVolume(audioSrcs, audioVols, tPercent);
            yield return new WaitForEndOfFrame();
        }
        foreach (GameObject obj in toReactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        yield return new WaitForSeconds(FADE_OUT_HOLD_TIME);

        ReturnToMenu();
    }

    private void SetAllVolume(List<AudioSource> audioSrcs, List<float> maxVols, float t)
    {
        int len = audioSrcs.Count;
        for (int i = 0; i < len; i++)
        {
            audioSrcs[i].volume = Mathf.Lerp(maxVols[i], 0, t);
        }
    }

    private void ClearWalls()
    {
        SetEverythingTransparency(1);

        foreach (Renderer rend in childRenderers)
        {
            rend.gameObject.SetActive(false);
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    private void addAudioTimer() {
        int time = MUserSettings.getTime();
        if(time >= 0) {
            foreach (GameObject audSource in audioToFade)
            {
                if (audSource.GetComponent<AudioSource>() != null)
                {
                    SoundTimer timer = audSource.AddComponent<SoundTimer>();
                    timer.time = time;
                }
            }
        }
    }
}
