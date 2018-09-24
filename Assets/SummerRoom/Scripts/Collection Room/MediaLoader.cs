using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using vrhome;

public class MediaLoader : MonoBehaviour
{

    public Transform start; //should be a little bit along the string, since a photo will spawn here
    public Transform mid; //should be further down from the actual midpoint (uses Bezier curve)
    public Transform end;
    //TODO: support more than one string?

    public Transform recordSpawn;
    public float recordHeight = 0.05f;

    public GameObject photoPrefab;
    public GameObject videoPrefab;
    public GameObject audioPrefab;

    private void Start()
    {
        //load photos and videos along the string
        Vector3 startPos = start.position;
        Vector3 midPos = mid.position;
        Vector3 endPos = end.position;

        Texture2D[] textures = Resources.LoadAll<Texture2D>("Media");
        VideoClip[] videos = Resources.LoadAll<VideoClip>("Media");

        float i = 0;
        int numPhotos = textures.Length + videos.Length;

        foreach (Texture2D tex in textures)
        {
            GameObject photo = Instantiate(photoPrefab);
            //photo.GetComponent<ImageQuad>().SetTexture(tex);
            photo.transform.position = GetLinePos(startPos, midPos, endPos, i / numPhotos);
            photo.GetComponent<Rigidbody>().isKinematic = true;
            i++;
            CollectionData.addToImages(tex.name, photo);
        }

        foreach (VideoClip vid in videos)
        {
            GameObject video = Instantiate(videoPrefab);
            video.GetComponent<VideoQuad>().SetVideo(vid);
            video.transform.position = GetLinePos(startPos, midPos, endPos, i / numPhotos);
            video.GetComponent<Rigidbody>().isKinematic = true;
            i++;
            CollectionData.addToVideos(vid.name, video);
        }

        //load records in a stack
        float height = recordSpawn.position.y;
        AudioClip[] audios = Resources.LoadAll<AudioClip>("Media");
        foreach (AudioClip ac in audios)
        {
            GameObject audio = Instantiate(audioPrefab);
            audio.GetComponent<Record>().SetAudio(ac);
            audio.transform.position = new Vector3(recordSpawn.position.x, height, recordSpawn.position.z);
            height += recordHeight;
            CollectionData.addToSounds(ac.name, audio);
        }

        gameObject.GetComponent<SaveLoad>().Load();
        gameObject.GetComponent<SaveLoadMedia>().Load();
    }

    private Vector3 GetLinePos(Vector3 startPos, Vector3 midPos, Vector3 endPos, float t)
    {
        Vector3 a = Vector3.Lerp(startPos, midPos, t);
        Vector3 b = Vector3.Lerp(midPos, endPos, t);
        return Vector3.Lerp(a, b, t);
    }
}
