using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Could be changed to be something for unquie later, but should function for now.
// Basically, distinguishing original objects in the scene by their names and default coordinates.
[System.Serializable]
public class ID  {
    [SerializeField] public string objName;
    [SerializeField] public float objCoordX;
    [SerializeField] public float objCoordY;
    [SerializeField] public float objCoordZ;
}
