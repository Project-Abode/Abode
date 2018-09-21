using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ResizeVideo : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    private Vector2 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(ResizeWhenLoaded());
    }
    
    private IEnumerator ResizeWhenLoaded()
    {
        while (!videoPlayer.isPrepared)
        {
            //video is still loading
            yield return null;
        }

        float width = originalScale.x;
        float height = originalScale.y;

        Texture videoTexture = videoPlayer.texture;
        float aspect = (float)videoTexture.width / videoTexture.height;
        if (aspect > 1)
        {
            height /= aspect;
        }
        else
        {
            width *= aspect;
        }

        transform.localScale = new Vector3(width, height, 1);
    }
}
