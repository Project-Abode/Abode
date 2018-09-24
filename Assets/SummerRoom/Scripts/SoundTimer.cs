using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to a game object with an audio source to have its audio fade out after a certain amount of time.
public class SoundTimer : MonoBehaviour
{

    public int time; //in seconds.

    private int timeLeft;
    private AudioSource[] audio;

    void Start()
    {
        audio = this.GetComponents<AudioSource>();
        timeLeft = time;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
        }
        //Fade music out once time is up.
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        //TODO: prevent user from being able to adjust volume at this point.
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.4f);
            foreach (AudioSource sound in audio)
            {
                if (sound.volume > 0) sound.volume -= 0.01f;
            }
        }
        //Stops audio components.
        foreach (AudioSource sound in audio)
        {
            sound.Stop();
        }
    }

}
