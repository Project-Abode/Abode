using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpdatedObjects {

    private static Dictionary<int, string> createdObj = new Dictionary<int, string>();
    private static List<int> destroyedObj = new List<int>();

    public static void addToCreated(int created, string from) {
        createdObj.Add(created, from);
    }

    public static Dictionary<int, string> getCreatedObj() {
        return createdObj;
    }

    public static void addToDestroyed(int destroyed) {
        destroyedObj.Add(destroyed);
    }

    public static List<int> getDestroyedObj() {
        return destroyedObj;
    }
}