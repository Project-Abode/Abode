using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace vrhome
{
    public class SaveLoadCloud : MonoBehaviour
    {

        public SaveLoad mainSaveLoad;

        private SaveCloud[] toSave;

        private string userToLoad;
        private bool saveNew;
        private bool loadPrevious;

        void Start()
        {
            userToLoad = mainSaveLoad.userToLoad;
            saveNew = mainSaveLoad.saveNew;
            loadPrevious = mainSaveLoad.loadPrevious;
        }

        private void OnApplicationQuit()
        {
            if (saveNew) Save();
        }

        public void Save()
        {
            SaveClouds();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".clouds");
            bf.Serialize(file, toSave);
            file.Close();
        }

        public void SaveClouds()
        {
            List<SaveCloud> saveClouds = new List<SaveCloud>();

            foreach (KeyValuePair<string, GameObject> cloud in CollectionData.getClouds())
            {
                SaveCloud sc = new SaveCloud();
                GameObject obj = cloud.Value;

                sc.fileName = cloud.Key;
                sc.objActive = obj.activeSelf;
                sc.xPosition = obj.transform.localPosition.x;
                sc.yPosition = obj.transform.localPosition.y;
                sc.zPosition = obj.transform.localPosition.z;
                sc.xRotation = obj.transform.localEulerAngles.x;
                sc.yRotation = obj.transform.localEulerAngles.y;
                sc.zRotation = obj.transform.localEulerAngles.z;
                sc.xScale = obj.transform.localScale.x;
                sc.yScale = obj.transform.localScale.y;
                sc.zScale = obj.transform.localScale.z;

                saveClouds.Add(sc);
            }

            toSave = saveClouds.ToArray();
        }

        public void Load()
        {
            if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".clouds") && loadPrevious)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".clouds", FileMode.Open);
                SaveCloud[] clouds = (SaveCloud[])bf.Deserialize(file);
                file.Close();

                // Find data for the specific user, then load those objects in.
                // Otherwise, we'll start with the original, default scene.
                if (clouds != null)
                {
                    LoadClouds(clouds);
                }
            }
        }

        public void LoadClouds(SaveCloud[] clouds)
        {
            Dictionary<string, GameObject> cloudLoaded = CollectionData.getClouds();

            foreach (SaveCloud cloud in clouds)
            {
                GameObject obj;
                cloudLoaded.TryGetValue(cloud.fileName, out obj);

                if (obj != null)
                {
                    obj.SetActive(cloud.objActive);
                    obj.transform.localPosition = new Vector3(cloud.xPosition, cloud.yPosition, cloud.zPosition);
                    obj.transform.localEulerAngles = new Vector3(cloud.xRotation, cloud.yRotation, cloud.zRotation);
                    obj.transform.localScale = new Vector3(cloud.xScale, cloud.yScale, cloud.zScale);
                }
            }
        }
    }
}


