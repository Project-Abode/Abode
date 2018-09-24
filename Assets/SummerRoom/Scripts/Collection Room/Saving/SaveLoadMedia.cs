using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

using vrhome;

public class SaveLoadMedia : MonoBehaviour {

    private SaveObject[] imgToSave;
    private SaveObject[] vidToSave;
    private SaveObject[] soundToSave;
    private SaveObject[] canvasToSave;

    private List<GameObject> allCanvases = new List<GameObject>();

    private string userToLoad;
    private bool saveNew;
    private bool loadPrevious;

	private void Start()
	{
        userToLoad = gameObject.GetComponent<SaveLoad>().userToLoad;
        saveNew = gameObject.GetComponent<SaveLoad>().saveNew;
        loadPrevious = gameObject.GetComponent<SaveLoad>().loadPrevious;

        // Find all the canvases in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (obj.GetComponent<FrameManager>() != null) {
                allCanvases.Add(obj);
                Debug.Log("Found a Canvas");
            } 
        }
	}

	private void OnApplicationQuit()
	{
        if(saveNew) Save();
	}

	private void Save() {
        SaveMedia();
        SaveMedia current = new SaveMedia();
        current.images = imgToSave;
        current.videos = vidToSave;
        current.sounds = soundToSave;
        current.canvases = canvasToSave;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".media");
        bf.Serialize(file, current);
        file.Close();
    }

    private void SaveMedia() {
        List<SaveObject> saveImages = new List<SaveObject>();
        List<SaveObject> saveVideos = new List<SaveObject>();
        List<SaveObject> saveSounds = new List<SaveObject>();
        List<SaveObject> saveCanvas = new List<SaveObject>();

        foreach(KeyValuePair<string, GameObject> img in CollectionData.getImages())
        {
            AddMediaToList(img, saveImages);
        }
        foreach (KeyValuePair<string, GameObject> vid in CollectionData.getVideos())
        {
            AddMediaToList(vid, saveVideos);
        }
        foreach (KeyValuePair<string, GameObject> sound in CollectionData.getSounds())
        {
            AddMediaToList(sound, saveSounds);
        }
        foreach(GameObject canvas in allCanvases) {
            AddCanvasToList(canvas, saveCanvas);
        }
        imgToSave = saveImages.ToArray();
        vidToSave = saveVideos.ToArray();
        soundToSave = saveSounds.ToArray();
        canvasToSave = saveCanvas.ToArray();
    }

    private void AddMediaToList(KeyValuePair<string, GameObject> media, List<SaveObject> list) {
        SaveObject so = new SaveObject();
        GameObject obj = media.Value;

        so.objActive = obj.activeSelf;
        so.xPosition = obj.transform.localPosition.x;
        so.yPosition = obj.transform.localPosition.y;
        so.zPosition = obj.transform.localPosition.z;
        so.xRotation = obj.transform.localEulerAngles.x;
        so.yRotation = obj.transform.localEulerAngles.y;
        so.zRotation = obj.transform.localEulerAngles.z;
        so.xScale = obj.transform.localScale.x;
        so.yScale = obj.transform.localScale.y;
        so.zScale = obj.transform.localScale.z;

        if (obj.transform.childCount > 0)
        {
            Transform child = obj.transform.GetChild(0);
            Renderer quad = child.gameObject.GetComponent<Renderer>();
            if (quad != null && quad.material.mainTexture != null && quad.material.mainTexture.name != "") so.texture = quad.material.mainTexture.name;
            VideoPlayer player = child.gameObject.GetComponent<VideoPlayer>();
            if (player != null && player.clip != null && player.clip.name != "") so.video = player.clip.name;
        }

        Record sound = obj.GetComponent<Record>();
        if (sound != null && sound.GetAudio() != null && sound.GetAudio().name != "") so.audio = sound.GetAudio().name;

        list.Add(so);
    }

    private void AddCanvasToList(GameObject obj, List<SaveObject> list) {
        SaveObject saveObject = new SaveObject();

        ID id = new ID();
        id.objName = obj.name;
        id.objCoordX = obj.transform.position.x;
        id.objCoordY = obj.transform.position.y;
        id.objCoordZ = obj.transform.position.z;
        saveObject.objID = id;

        GameObject canvas = obj.transform.Find("Canvas").gameObject;
        if(canvas != null) {
            Renderer rend = canvas.GetComponent<Renderer>();
            VideoPlayer vid = canvas.GetComponent<VideoPlayer>();
            if(rend != null && rend.material != null && rend.material.GetTexture("_DisplayTex") != null && rend.material.GetTexture("_DisplayTex").name != "")
                saveObject.texture = rend.material.GetTexture("_DisplayTex").name;
            if(vid != null && vid.clip != null && vid.clip.name != "")
                saveObject.video = vid.clip.name;
        }

        list.Add(saveObject);
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".media") && loadPrevious)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".media", FileMode.Open);
            SaveMedia media = (SaveMedia)bf.Deserialize(file);
            file.Close();

            // Find data for the specific user, then load those objects in.
            // Otherwise, we'll start with the original, default scene.
            if (media != null)
            {
                LoadMedia(media);
            }
        }
    }

    private void LoadMedia(SaveMedia media) {
        SaveObject[] images = media.images;
        SaveObject[] videos = media.videos;
        SaveObject[] sounds = media.sounds;
        SaveObject[] canvases = media.canvases;


        Dictionary<string, GameObject> imgLoaded = CollectionData.getImages();
        Dictionary<string, GameObject> vidLoaded = CollectionData.getVideos();
        Dictionary<string, GameObject> soundLoaded = CollectionData.getSounds();

        foreach(SaveObject img in images) {
            GameObject obj;
            imgLoaded.TryGetValue(img.texture, out obj);

            if(obj != null) {
                SetObject(obj, img);
            }
        }

        foreach (SaveObject vid in videos)
        {
            GameObject obj;
            vidLoaded.TryGetValue(vid.video, out obj);

            if (obj != null)
            {
                SetObject(obj, vid);
            }
        }

        foreach (SaveObject sound in sounds)
        {
            GameObject obj;
            soundLoaded.TryGetValue(sound.audio, out obj);

            if (obj != null)
            {
                SetObject(obj, sound);
            }
        }

        foreach (SaveObject canvas in canvases) {
            Debug.Log("Trying to find canvas");

            GameObject obj = FindFromAll(canvas.objID);

            if(obj != null) {
                GameObject c = obj.transform.Find("Canvas").gameObject;
                Renderer rend = c.GetComponent<Renderer>();
                VideoPlayer vid = c.GetComponent<VideoPlayer>();

                if(rend != null && rend.material != null && rend.material.GetTexture("_DisplayTex") != null && Resources.Load("Media/" + canvas.texture) != null) {
                    Debug.Log("Found saved canvas texture: " + canvas.texture);

                    rend.material.SetTexture("_DisplayTex", (Texture)Resources.Load("Media/" + canvas.texture));
                    rend.material.SetFloat("_Threshold", 1.0f);
                }
                if(vid != null && Resources.Load("Media/" + canvas.audio) != null)
                    c.GetComponent<VideoPlayer>().clip = (VideoClip)(Resources.Load("Media/" + canvas.audio));
            }
        }
    }

    private void SetObject(GameObject obj, SaveObject reference) {
        obj.SetActive(reference.objActive);
        obj.transform.localPosition = new Vector3(reference.xPosition, reference.yPosition, reference.zPosition);
        obj.transform.localEulerAngles = new Vector3(reference.xRotation, reference.yRotation, reference.zRotation);
        obj.transform.localScale = new Vector3(reference.xScale, reference.yScale, reference.zScale);
    }

    private GameObject FindFromAll(ID toFind)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == toFind.objName
                && (int)(obj.transform.position.x * 100.0) == (int)(toFind.objCoordX * 100.0)
                && (int)(obj.transform.position.y * 100.0) == (int)(toFind.objCoordY * 100.0)
                && (int)(obj.transform.position.z * 100.0) == (int)(toFind.objCoordZ * 100.0))
            {
                return obj;
            }
        }
        return null;
    }
}
