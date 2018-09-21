using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{

    private const float FADE_SPEED = 0.03f;

    public GameObject fallingStone;

    private AudioSource audioSrc;
    private Renderer stoneRenderer;

	private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.Play();
        stoneRenderer = fallingStone.GetComponent<Renderer>();
	}

    private void Update()
    {
        Color oldCol = stoneRenderer.material.color;
        stoneRenderer.material.color = new Color(oldCol.r, oldCol.g, oldCol.b, oldCol.a - FADE_SPEED);

        if (!audioSrc.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
