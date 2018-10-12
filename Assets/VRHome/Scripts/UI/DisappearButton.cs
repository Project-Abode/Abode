using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject canvas;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            canvas.SetActive(false);
        }
    }
}
