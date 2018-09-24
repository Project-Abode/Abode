using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveCloud {
    [SerializeField] public string fileName;
    [SerializeField] public bool objActive;
    [SerializeField] public float xPosition;
    [SerializeField] public float yPosition;
    [SerializeField] public float zPosition;
    [SerializeField] public float xRotation;
    [SerializeField] public float yRotation;
    [SerializeField] public float zRotation;
    [SerializeField] public float xScale;
    [SerializeField] public float yScale;
    [SerializeField] public float zScale;
}
