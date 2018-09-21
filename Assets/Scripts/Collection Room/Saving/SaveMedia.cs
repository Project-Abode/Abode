using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveMedia
{
    [SerializeField] public SaveObject[] images;
    [SerializeField] public SaveObject[] videos;
    [SerializeField] public SaveObject[] sounds;
    [SerializeField] public SaveObject[] canvases;
}