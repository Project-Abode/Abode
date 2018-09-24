using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitText : MonoBehaviour {

    private bool startTimer = false;
    private int timeLeft;

	private void Start()
    {
        timeLeft = MUserSettings.getTime();
        if (timeLeft < 0) Destroy(this.gameObject);
    }

	void Update () {
        if (startTimer)
        {
            startTimer = false;
            StartCoroutine(Countdown());
        }
	}
	
    private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
        }
        //Fade music out once time is up.
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        List<Renderer> toFadeIn = new List<Renderer>();

        foreach (Transform child in transform)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null)
            {
                toFadeIn.Add(rend);
            }
        }

        //First make sure all are trasparent.
        foreach (Renderer rend in toFadeIn) {
            Color color = rend.material.color;
            rend.material.color = new Color(color.r, color.g, color.b, 0f);
        }

        // Reactivate each object to fade in.
        foreach (Renderer rend in toFadeIn)
        {
            rend.gameObject.SetActive(true);
        }

        // Fadein elements.
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.07f);

            foreach (Renderer rend in toFadeIn)
            {
                Color color = rend.material.color;
                rend.material.color = new Color(color.r, color.g, color.b, color.a + 0.05f);
            }
        }
    }

    public void setStartTimer(bool x) {
        startTimer = x;
    }
}
