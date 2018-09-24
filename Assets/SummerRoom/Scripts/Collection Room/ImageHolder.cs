using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ImageHolder : Holder
{

    private Renderer rend;
    private VideoPlayer vp;
    private Texture defaultTexture;

    protected override void Start()
    {
        base.Start();
        rend = GetComponent<Renderer>();
        vp = GetComponent<VideoPlayer>();
        defaultTexture = rend.material.mainTexture;
    }

    public override void Apply(GameObject obj)
    {
        Transform t = obj.transform.Find("Quad");
        if (t != null)
        {
            base.Apply(obj);
            GameObject quad = t.gameObject;
            VideoPlayer vid = quad.GetComponent<VideoPlayer>();
            if (vid != null)
            {
                vp.clip = vid.clip;
                vp.Play();
            }
            else
            {
                //use image texture
                rend.material.mainTexture = quad.GetComponent<Renderer>().material.mainTexture;
            }
        }
    }

    protected override void Remove()
    {
        base.Remove();

        vp.Stop();
        vp.clip = null;
        rend.material.mainTexture = defaultTexture;
    }
}
