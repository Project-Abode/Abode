using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watersound : MonoBehaviour {
    public AudioSource water;
	// Use this for initialization
	void Start () {
		
	}
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Coin(Clone)")
        {
            water.Play();
        }
        
    }

}
