using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PreMeditationMenu : MonoBehaviour
{

    public ToggleGroup timeSelect; //Possible times.
    public ToggleGroup environSelect; //Possible environments.
    public string nextScene;

    private float WELCOME_LOAD_TIME = 2f;
    private bool timeSet = false; //Make sure time only gets set once.
    private bool environSet = false; //Make sure environment only gets set once.

    private void Start()
    {
        // Make sure the scene starts with only the "Welcome" text and menu backdrop.
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        transform.Find("Background").gameObject.SetActive(true);
        transform.Find("Welcome").gameObject.SetActive(true);

        // Start option-select sequence.
        StartCoroutine(Welcome());
    }

    private IEnumerator Welcome()
    {
        yield return new WaitForSeconds(WELCOME_LOAD_TIME);

        StartCoroutine(fade(this.gameObject.transform.Find("Welcome").gameObject, this.gameObject.transform.Find("Time").gameObject, false));
    }

	private void Update()
	{
        // Check if the user has selected a time, and then store that for room generation.
        if (timeSelect.AnyTogglesOn() && !timeSet)
        {
            setTime();
            StartCoroutine(fade(this.gameObject.transform.Find("Time").gameObject, this.gameObject.transform.Find("Environment").gameObject, false));
        }

        // Check if the user has selected an environment, and then store that for room generation.
        if (environSelect.AnyTogglesOn() && !environSet)
        {
            setEnvironment();

            StartCoroutine(fade(this.gameObject.transform.Find("Environment").gameObject, null, true));

        }
	}

    private void setTime()
    {
        string timeActive = timeSelect.ActiveToggles().FirstOrDefault().name;
        switch (timeActive)
        {
            case "5":
                MUserSettings.setTime(300);
                break;
            case "10":
                MUserSettings.setTime(600);
                break;
            case "20":
                MUserSettings.setTime(1200);
                break;
            case "inf":
                MUserSettings.setTime(-1); //Use negative value to indicate infinite time.
                break;
        }
        timeSet = true;
    }

    private void setEnvironment()
    {
        //Set-up, to potentially add different environments user can choose from.
        MUserSettings.setEnviron(environSelect.ActiveToggles().FirstOrDefault().name);
        environSet = false;
    }

    //Returns all the MeshRenderers under an object and its children.
    private List<Text> getChildText (Transform parent, List<Text> list) {
        foreach(Transform child in parent) {
            if (child == null) continue; //No more children to recurse on so break.

            Text text = child.GetComponent<Text>();
            if (text != null) list.Add(text);
            getChildText(child, list);
        }
        return list;
    }

    // Could later attempt to generalize this code to work for all UI elements, not just text.
    private IEnumerator fade(GameObject toFadeOut, GameObject toFadeIn, bool endScene)
    {
        Image infNorm = null;
        Image infIta = null;

        if(toFadeOut != null && toFadeOut.gameObject.name == "Time") {
            infNorm = toFadeOut.transform.Find("inf").transform.Find("normal").GetComponent<Image>();
            infIta = toFadeOut.transform.Find("inf").transform.Find("highlight").GetComponent<Image>();
        }

        if(toFadeIn != null && toFadeIn.gameObject.name == "Time") {
            infNorm = toFadeIn.transform.Find("inf").transform.Find("normal").GetComponent<Image>();
            infIta = toFadeIn.transform.Find("inf").transform.Find("highlight").GetComponent<Image>();
        }

        if (toFadeOut != null)
        {
            // Get all elements under the object to fade out that have text.
            List<Text> allOutText = getChildText(toFadeOut.transform, new List<Text>());
            Text outParentText = toFadeOut.GetComponent<Text>();
            if (outParentText != null) allOutText.Add(outParentText);
            int allOTCount = allOutText.Count;

            // Fadeout elements.
            for (int i = 0; i < 20; i++)
            {
                yield return new WaitForSeconds(0.07f);
                //Special case on infinity symbol.
                if(toFadeOut.gameObject.name == "Time") {
                    Color color = infNorm.color;
                    infNorm.color = new Color(color.r, color.g, color.b, color.a - 0.05f);
                    infIta.color = new Color(color.r, color.g, color.b, color.a - 0.05f);
                }
                for (int j = 0; j < allOTCount; j++)
                {
                    Color color = allOutText[j].color;
                    allOutText[j].color = new Color(color.r, color.g, color.b, color.a - 0.05f);
                }
            }

            // Disable first object.
            toFadeOut.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        if (toFadeIn != null)
        {
            // Get all elements under the object to fade in that have text.
            List<Text> allInText = getChildText(toFadeIn.transform, new List<Text>());
            Text inParentText = toFadeIn.GetComponent<Text>();
            if (inParentText != null) allInText.Add(inParentText);
            int allITCount = allInText.Count;

            // Set all text elements to transparent first.
            for (int j = 0; j < allITCount; j++)
            {
                Color color = allInText[j].color;
                allInText[j].color = new Color(color.r, color.g, color.b, 0f);
            }
            //Special case on infinity symbol.
            if (toFadeIn.gameObject.name == "Time")
            {
                Color color = infNorm.color;
                infNorm.color = new Color(color.r, color.g, color.b, 0f);
                infIta.color = new Color(color.r, color.g, color.b, 0f);
            }
            // Reactivate the object to fade in.
            toFadeIn.SetActive(true);

            // Fadein elements.
            for (int i = 0; i < 20; i++)
            {
                yield return new WaitForSeconds(0.07f);
                //Special case on infinity symbol.
                if (toFadeIn.gameObject.name == "Time")
                {
                    Color color = infNorm.color;
                    infNorm.color = new Color(color.r, color.g, color.b, color.a + 0.05f);
                    infIta.color = new Color(color.r, color.g, color.b, color.a + 0.05f);
                }
                for (int j = 0; j < allITCount; j++)
                {
                    Color color = allInText[j].color;
                    allInText[j].color = new Color(color.r, color.g, color.b, color.a + 0.05f);
                }
            }
        }

        if(endScene) SceneManager.LoadScene(nextScene); //Move into the next room when indicated.
    }

}
