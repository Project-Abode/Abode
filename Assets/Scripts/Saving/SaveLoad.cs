using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace vrhome{ 
public class SaveLoad : MonoBehaviour {

    // Cheap way to indicate what user we want to load for now.
    public string userToLoad;
    // Some bools for debugging purposes; whether or not to actively save/load.
    public bool saveNew;
    public bool loadPrevious;

    private Room saveRoom;
    private Dictionary<int, ID> originalObjID = new Dictionary<int, ID>(); // Save reference to original objects (via instance IDs) and their IDs in the scene.
    private SaveObject[] objToSave;
    private SavePrefab[] prefabToSave;
    private ID[] objToDestroy;
    private int toSaveCount;
    private int numToSave;

    public void OnApplicationQuit()
    {
        if(saveNew) Save();
	}

	public void Save() {
        SaveObjects();
        Room current = new Room();
        //Save the current room by saving the user and the transform of ST objects.
        current.user = userToLoad;
        current.objects = objToSave;
        current.prefabs = prefabToSave;
        current.destroyObjects = objToDestroy;

        // Save the updated room.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm");
        bf.Serialize(file, current);
        file.Close();
    }

    private void SaveObjects() {
        // Set a SaveObject or SavePrefab for everything in scene.
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); // Get all game objects currently in the scene.
        List<SaveObject> saveObjects = new List<SaveObject>();
        List<SavePrefab> savePrefabs = new List<SavePrefab>();
        List<ID> saveDestroyed = new List<ID>();
        foreach (GameObject obj in allObjects)
        {
            if (UpdatedObjects.getCreatedObj().ContainsKey(obj.GetInstanceID()) && !UpdatedObjects.getDestroyedObj().Contains(obj.GetInstanceID()))
            { // Otherwise, add to loaded prefabs.

                SavePrefab sp = new SavePrefab();
                string val;
                UpdatedObjects.getCreatedObj().TryGetValue(obj.GetInstanceID(), out val);
                sp.prefabToLoad = val;
                sp.objData = CreateSaveObj(obj);

                List<SaveObject> loadPrefabChildren = new List<SaveObject>();
                foreach (Transform child in obj.transform)
                {
                    SaveObject spc = CreateSaveObj(child.gameObject);
                    loadPrefabChildren.Add(spc);
                }

               sp.childrenData = loadPrefabChildren.ToArray();
               savePrefabs.Add(sp);

            } else if (UpdatedObjects.getDestroyedObj().Contains(obj.GetInstanceID())){
                ID id;
                originalObjID.TryGetValue(obj.GetInstanceID(), out id);
                if (id != null) saveDestroyed.Add(id);
            } else if (originalObjID.ContainsKey(obj.GetInstanceID()))
            { // Add to SaveObjects if originally in scene.
                SaveObject so = CreateSaveObj(obj);

                saveObjects.Add(so);
            }
        }
        objToSave = saveObjects.ToArray();
        prefabToSave = savePrefabs.ToArray();
        objToDestroy = saveDestroyed.ToArray();
    }

    private SaveObject CreateSaveObj(GameObject obj) {
        SaveObject save = new SaveObject();
        ID id;
        originalObjID.TryGetValue(obj.GetInstanceID(), out id);
        save.objID = id;

        save.objActive = obj.activeSelf;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) save.objKinematic = rb.isKinematic;
        save.objActive = obj.activeSelf;
        save.xPosition = obj.transform.localPosition.x;
        save.yPosition = obj.transform.localPosition.y;
        save.zPosition = obj.transform.localPosition.z;
        save.xRotation = obj.transform.localEulerAngles.x;
        save.yRotation = obj.transform.localEulerAngles.y;
        save.zRotation = obj.transform.localEulerAngles.z;
        save.xScale = obj.transform.localScale.x;
        save.yScale = obj.transform.localScale.y;
        save.zScale = obj.transform.localScale.z;
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null && rend.material.mainTexture != null && rend.material.mainTexture.name != "") save.texture = rend.material.mainTexture.name;
        VideoPlayer player = obj.GetComponent<VideoPlayer>();
        if (player != null && player.clip != null && player.clip.name != "") save.video = player.clip.name;
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source != null && source.clip != null && source.clip.name != "") save.audio = source.clip.name;

        return save;
    }

    private SaveObject CreateSavePrefab(GameObject obj) {
        SaveObject save = new SaveObject();
        ID id = new ID();
        id.objName = obj.name;
        id.objCoordX = obj.transform.position.x;
        id.objCoordY = obj.transform.position.y;
        id.objCoordZ = obj.transform.position.z;
        save.objID = id;
        save.objActive = obj.activeSelf;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) save.objKinematic = rb.isKinematic;
        save.xPosition = obj.transform.localPosition.x;
        save.yPosition = obj.transform.localPosition.y;
        save.zPosition = obj.transform.localPosition.z;
        save.xRotation = obj.transform.localEulerAngles.x;
        save.yRotation = obj.transform.localEulerAngles.y;
        save.zRotation = obj.transform.localEulerAngles.z;
        save.xScale = obj.transform.localScale.x;
        save.yScale = obj.transform.localScale.y;
        save.zScale = obj.transform.localScale.z;
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null && rend.material.mainTexture != null && rend.material.mainTexture.name != "") save.texture = rend.material.mainTexture.name;
        VideoPlayer player = obj.GetComponent<VideoPlayer>();
        if (player != null && player.clip != null && player.clip.name != "") save.video = player.clip.name;
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source != null && source.clip != null && source.clip.name != "") save.audio = source.clip.name;

        return save;
    }

    // Loads the correct room given user index.
    // Else, loads a default.
    // Use a negative index to indicate it is a new user.
    public void Load()
    {
        // Save what objects were originally in the scene (i.e. not from a prefab/created during runtime).
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<int> originalObjects = allObjectInstanceID(allObjects);
        // Generate the IDs of objects.
        foreach (GameObject obj in allObjects)
        {
            ID id = new ID();
            id.objName = obj.name;
            id.objCoordX = obj.transform.position.x;
            id.objCoordY = obj.transform.position.y;
            id.objCoordZ = obj.transform.position.z;
            originalObjID.Add(obj.GetInstanceID(), id);
        }

        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm") && loadPrevious)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm", FileMode.Open);
            saveRoom = (Room)bf.Deserialize(file);
            file.Close();

            // Find data for the specific user, then load those objects in.
            // Otherwise, we'll start with the original, default scene.
            if (saveRoom != null)
            {
                LoadObjects(saveRoom);
            }
        }
    }

    private void LoadObjects(Room saveRoom)
    {
        // Set the saved transforms of objects.
        SaveObject[] loadObjState = saveRoom.objects;
        if (loadObjState != null)
        {
            foreach (SaveObject so in loadObjState)
            {
                GameObject toLoad = FindFromAll(so.objID);

                if (toLoad != null)
                {
                    SetObject(toLoad, so);
                }
            }
        }

        // Load in any prefab objects created previously.
        SavePrefab[] loadPrefabState = saveRoom.prefabs;
        if (loadPrefabState != null)
        {
            foreach (SavePrefab lp in loadPrefabState)
            {
                GameObject loaded = (GameObject)Instantiate(Resources.Load(lp.prefabToLoad));
                UpdatedObjects.addToCreated(loaded.GetInstanceID(), lp.prefabToLoad);
                SetObject(loaded, lp.objData);

                // Arrange the children of the prefab.
                foreach (SaveObject lpc in lp.childrenData)
                {
                    GameObject loadChild = loaded.transform.Find(lpc.objID.objName).gameObject;

                    if (loadChild != null)
                    {
                        SetObject(loadChild, lpc);
                    }
                }
            }
        }


        // Destroy any gameobjects that the user deleted from the default.
        ID[] toDestroy = saveRoom.destroyObjects;
        if (toDestroy != null)
        {
            foreach (ID td in toDestroy){
                GameObject destroy = FindFromAll(td);
                if (destroy != null) Destroy(destroy);
            }
        }
    }

    private void SetObject(GameObject obj, SaveObject reference)
    {
        obj.SetActive(reference.objActive);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = reference.objKinematic;
        obj.transform.localPosition = new Vector3(reference.xPosition, reference.yPosition, reference.zPosition);
        obj.transform.localEulerAngles = new Vector3(reference.xRotation, reference.yRotation, reference.zRotation);
        obj.transform.localScale = new Vector3(reference.xScale, reference.yScale, reference.zScale);
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null && reference.texture != null && Resources.Load("Media/" + reference.texture) != null) rend.material.mainTexture = (Texture2D)(Resources.Load("Media/" + reference.texture));
        VideoPlayer player = obj.GetComponent<VideoPlayer>();
        if (player != null && reference.video != null && Resources.Load("Media/" + reference.audio) != null) player.clip = (VideoClip)(Resources.Load("Media/" + reference.video));
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source != null && reference.audio != null && Resources.Load("Media/" + reference.audio) != null) source.clip = (AudioClip)(Resources.Load("Media/" + reference.audio));
    }

    private List<int> allObjectInstanceID(GameObject[] objects)
    {
        List<int> objIDs = new List<int> ();
        foreach (GameObject obj in objects)
        {
            objIDs.Add(obj.GetInstanceID());
        }
        return objIDs;
    }

    private GameObject FindFromAll(ID toFind) {

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {

            if (obj.name == toFind.objName
                && (int)(obj.transform.position.x * 100.0) == (int)(toFind.objCoordX * 100.0)
                && (int)(obj.transform.position.y * 100.0) == (int)(toFind.objCoordY * 100.0)
                && (int)(obj.transform.position.z * 100.0) == (int)(toFind.objCoordZ * 100.0)) {
                return obj;
            }
        }
        return null;
    }
}
}