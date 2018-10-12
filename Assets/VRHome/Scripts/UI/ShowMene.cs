using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public GameObject canvas;

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
	}
}
