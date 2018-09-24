using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    private AudioSource soundEffect;

    public void Awake()
    {
        soundEffect = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (!soundEffect.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}