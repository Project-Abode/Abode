using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebImage : ImageQuad
{

    public string imageUrl;

    private void Start()
    {
        StartCoroutine(DownloadImage());
    }

    private IEnumerator DownloadImage()
    {
        using (WWW www = new WWW(imageUrl))
        {
            yield return www;

            Texture2D texture = www.texture;
            //SetTexture(texture);
        }
    }
    
}
