using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetSkyHalf : MonoBehaviour {

    public float percentThroughDay;
    public Material dayToSunset;
    public Material sunsetToNight;
    public Color daylightColor = new Color(1, 0.9568f, 0.8392f, 1);
    public Color sunsetColor = new Color(1, 0.9682f, 0.6462f, 1);
    public Color nightColor = new Color(0.946f, 0.929f, 1, 1);
    public Light sunLight;
    public Light bounceLight;

    private Vector3 sunDefaultPositionVector = new Vector3(-10, 0, -135);

	// Use this for initialization
    void Start () {
        percentThroughDay = 0f; // starts with daylight
        applyChanges();
	}
	
	// Update is called once per frame
	void Update () {
        /* MANUAL TIME SHIFT: FOR DEBUGGING */
        if (Input.GetKey("right"))
        {
            percentThroughDay += 1f;
            if (percentThroughDay > 100)
            {
                percentThroughDay = 100;
            }
            //Debug.Log("percentThroughDay is " + percentThroughDay);
        }
        if (Input.GetKey("left"))
        {
            percentThroughDay -= 1f;
            if (percentThroughDay < 0)
            {
                percentThroughDay = 0;
            }
            //Debug.Log("percentThroughDay is " + percentThroughDay);
        }
        applyChanges();
	}

    public void applyChanges() {
        float angleLateral = (percentThroughDay / 2) + 220;
        if (percentThroughDay > 40)
        // After Sunset    Debug.Log("Could not find Quad");
        {
            float percentThroughEvening = (percentThroughDay - 40) / 60;
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", percentThroughEvening);
            float angleVertical = Mathf.Lerp(20, -5, percentThroughEvening);
            sunLight.transform.rotation = Quaternion.Euler(angleVertical, angleLateral, angleVertical);
            //sun.transform.position = sunLight.transform.rotation * sunDefaultPositionVector;
            sunLight.intensity = Mathf.Lerp(1.5f, 0, percentThroughEvening);
            sunLight.color = sunsetColor;
            bounceLight.color = Color.Lerp(sunsetColor, nightColor, percentThroughEvening);
            bounceLight.intensity = Mathf.Lerp(sunLight.intensity/2, 0.0f, percentThroughEvening);
        }
        else
        // Before Sunset    
        {
            float percentThroughMorning = percentThroughDay / 40;    
            RenderSettings.skybox = dayToSunset;
            dayToSunset.SetFloat("_Blend", percentThroughMorning);
            float angleVertical = Mathf.Lerp(60, 20, percentThroughMorning);
            sunLight.transform.rotation = Quaternion.Euler(angleVertical, angleLateral, angleVertical);
            //sun.transform.position = sunLight.transform.rotation * sunDefaultPositionVector;
            sunLight.intensity = 1.5f;
            Color lightColor = Color.Lerp(daylightColor, sunsetColor, percentThroughMorning);
            sunLight.color = lightColor;
            bounceLight.color = lightColor;
            bounceLight.intensity = sunLight.intensity/2;
        }
    }

    private void OnApplicationQuit()
    {
        percentThroughDay = 0f;
        applyChanges();
    }
}
