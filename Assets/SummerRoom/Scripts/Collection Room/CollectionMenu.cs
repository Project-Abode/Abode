using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionMenu : MonoBehaviour {

    public Button welcNext;
    public Button imgNext;
    public Button vidNext;
    public Button soundNext;
    public Button textNext;
    public Button finish;
    public string nextScene;

	// Use this for initialization
	void Start () {
        // Make sure the scene starts with "welcome".
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("background").gameObject.SetActive(true);
        transform.Find("welcome").gameObject.SetActive(true);

        // Set-up bubttonlisteners for transitions between each menu aspect.
        welcNext.onClick.AddListener(delegate { transition("welcome", "imgSelect"); });
        imgNext.onClick.AddListener(delegate { transition("imgSelect", "vidSelect"); });
        vidNext.onClick.AddListener(delegate { transition("vidSelect", "soundSelect"); });
        soundNext.onClick.AddListener(delegate { transition("soundSelect", "textSelect"); });
        textNext.onClick.AddListener(delegate { transition("textSelect", "finish"); });
        finish.onClick.AddListener(loadScene);
	}
	
    //Transitions between the two children given.
    private void transition (string from, string to) {
        transform.Find(from).gameObject.SetActive(false);
        transform.Find(to).gameObject.SetActive(true);
    }

    // Load the room once set-up is complete.
    private void loadScene () {
        SceneManager.LoadScene(nextScene);
    }
}
