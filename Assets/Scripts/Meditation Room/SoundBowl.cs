using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBowl : Bowl
{

    private float adjustFactor;

    private AudioSource[] audioSrcs;
    private float globalVolume;
    private float[] maxVolumes;

    private void Start()
    {
        adjustFactor = Mathf.Log(10, 3);

        audioSrcs = GetComponents<AudioSource>();
        int len = audioSrcs.Length;
        maxVolumes = new float[len];
        for (int i = 0; i < len; i++)
        {
            maxVolumes[i] = audioSrcs[i].volume;
            audioSrcs[i].volume = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) SetValue(0.0f);
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetValue(0.25f);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetValue(0.5f);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetValue(0.75f);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetValue(1.0f);
    }

    protected override float GetValue()
    {
        return globalVolume;
    }

    protected override void SetValue(float newValue)
    {
        globalVolume = newValue;
        int len = audioSrcs.Length;
        for (int i = 0; i < len; i++)
        {
            float adjustedVol = Mathf.Pow(globalVolume, adjustFactor) * maxVolumes[i];
            audioSrcs[i].volume = adjustedVol;
        }
    }
}
