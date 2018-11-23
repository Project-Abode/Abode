using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour {
    public float time_passed;
    public float speed_of_motion;
    public float end_time;
    public GameObject fire_particle_effect;
	// Use this for initialization
	void Start () {
        time_passed = Time.time;
        //speed = 1.0f;
        //end = 300.0f;
	}
	
	// Update is called once per frame
	void Update () {
        time_passed = Time.time;
        if (Time.time  < end_time) { 
        transform.position += speed_of_motion * Vector3.down * Time.deltaTime/200;
        }
        if (Time.time > end_time)
        {
            Destroy(fire_particle_effect);
        }
    }
}
