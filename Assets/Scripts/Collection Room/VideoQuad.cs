using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoQuad : MediaQuad
{

    public GameObject vidObject;

    private VideoPlayer player;

    protected override void Awake()
    {
        base.Awake();

        if (vidObject == null)
        {
            vidObject = gameObject;
        }
        player = vidObject.GetComponent<VideoPlayer>();
    }

    public void SetVideo(VideoClip vid)
    {
        Scale(vid.width, vid.height);
        player.clip = vid;
    }

}
