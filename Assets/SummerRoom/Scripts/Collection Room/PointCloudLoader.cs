using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using vrhome;

public class PointCloudLoader : MonoBehaviour
{
    private const string PLY_SAVE_PATH = "Assets/Resources/PointClouds/";

    public GameObject pcPrefab;
    public Transform poolSpot;
    public float poolScale;

    private void Start()
    {
        Vector3 poolPos = poolSpot.transform.position;

        string[] paths = Directory.GetFiles(PLY_SAVE_PATH);
        foreach (string path in paths)
        {
            if (Path.GetExtension(path) != ".ply") continue;
            if (Path.GetFileNameWithoutExtension(path) == "head") continue;

            GameObject copy = Instantiate(pcPrefab);
            ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(path);
            KeepParticles keep = copy.transform.Find("PointCloudCopy").GetComponent<KeepParticles>();
            keep.SetParticles(plyParticles);
            Vector2 offset = Random.insideUnitCircle * poolScale;
            copy.transform.position = poolPos + new Vector3(offset.x, 0, offset.y);

            CollectionData.addToClouds(path, copy);
        }
        gameObject.GetComponent<SaveLoadCloud>().Load();
    }
}
