using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    private const string PLY_SAVE_PATH = "Assets/Resources/PointClouds/";

    private void Start()
    {
        ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(PLY_SAVE_PATH + "head.ply");
        GetComponent<KeepParticles>().SetParticles(plyParticles);
    }
}
