using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColliderManager : GameSingleton<WaterColliderManager> {

    List<WaterCollider> _colliders = new List<WaterCollider>();
    public int GetNumColliders()
    {
        return _colliders.Count;
    }
    public WaterCollider GetWaterCollider(int index)
    {
        return _colliders[index];
    }

    public void Register(WaterCollider wc)
    {
        _colliders.Add(wc);
    }

    public bool Unregister(WaterCollider wc)
    {
        return _colliders.Remove(wc);
    }
}
