using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace vrhome {
    [System.Serializable]
    public class Room
    {
        [SerializeField] public string user;
        //[SerializeField] public List<GameObject> objInRoom;
        [SerializeField] public SaveObject[] objects;
        [SerializeField] public SavePrefab[] prefabs;
        [SerializeField] public ID[] destroyObjects;
    }
}