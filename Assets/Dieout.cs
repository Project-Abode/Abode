using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dieout : MonoBehaviour {
    public float t;
    public float end;
    // Use this for initialization
    void Start () {
        t = Time.time;
        end = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - t > end)
        {
            Destroy(this.gameObject);
        }
    }
}
