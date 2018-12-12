using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Surprise : MonoBehaviour {
    //public GameObject surprise;
    public GameObject g1;
    public GameObject g2;
    public GameObject g3;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        float r = Random.Range(0.0f, 3.0f);
        if (r < 1)
            Instantiate(g1); 
        if(r>1 & r<2)
            Instantiate(g2);
        if(r>2)
            Instantiate(g3);
        Destroy(this.gameObject);
    }
}
