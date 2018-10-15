using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Watch : MonoBehaviour {
   
    public float hour;
    public float min;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hour = System.DateTime.Now.Hour;
        min = System.DateTime.Now.Minute;
        if (hour>10 & min>10)
            this.GetComponent<Text>().text = hour + ":" +min;

        if (hour > 10 & min < 10)
            this.GetComponent<Text>().text = hour + ":0" + min;

        if (hour < 10 & min < 10)
            this.GetComponent<Text>().text ="0"+ hour + ":0" + min;

        if (hour < 10 & min > 10)
            this.GetComponent<Text>().text = "0" + hour + ":" + min;
    }
}
