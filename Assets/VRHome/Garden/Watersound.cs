using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watersound : MonoBehaviour {
    public AudioSource water;
	// Use this for initialization
	void Start () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        water.Play();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
