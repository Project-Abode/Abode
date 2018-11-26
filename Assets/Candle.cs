using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public float time_passed;
    public float speed_of_burning;
    //public float end_time;
    public GameObject fire_particle_effect;
    // Use this for initialization
    void Start()
    {
        time_passed = Time.time;
        //speed = 1.0f;
        //end = 300.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time_passed = Time.time;
        if (transform.localScale.y > 0)
        {
            transform.localScale += speed_of_burning * Vector3.down * Time.deltaTime / 200;
        }
        if (transform.localScale.y < 0)
        {
            Destroy(fire_particle_effect);
        }
    }
}
