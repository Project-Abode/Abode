using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionData {

    private static Dictionary<string, GameObject> images = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> videos = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> sounds = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> pointClouds = new Dictionary<string, GameObject>();

    public static void addToImages(string texture, GameObject obj) {
        images.Add(texture, obj);
    }

    public static Dictionary<string, GameObject> getImages()
    {
        return images;
    }

    public static void addToVideos(string video, GameObject obj)
    {
        videos.Add(video, obj);
    }

    public static Dictionary<string, GameObject> getVideos()
    {
        return videos;
    }

    public static void addToSounds(string audio, GameObject obj)
    {
        sounds.Add(audio, obj);
    }

    public static Dictionary<string, GameObject> getSounds()
    {
        return sounds;
    }

    public static void addToClouds(string cloud, GameObject obj)
    {
        pointClouds.Add(cloud, obj);
    }

    public static Dictionary<string, GameObject> getClouds()
    {
        return pointClouds;
    }
}
