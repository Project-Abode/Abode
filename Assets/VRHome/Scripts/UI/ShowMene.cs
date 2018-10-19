using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMene : MonoBehaviour {

    public GameObject canvas;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
	}

	public void ShowTV() {
		canvas.SetActive(true);
	}

}
