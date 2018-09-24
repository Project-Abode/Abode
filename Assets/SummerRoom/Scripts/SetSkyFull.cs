using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkyFull : MonoBehaviour {

    private AudioSource ambientSound;

    //NIGHTTIME AMBIENT SOUND
    public AudioClip ambientNight;
    private float volumehiN = 0.7f;
    private float volumeloN = 0f;
    private float volumeindN = 0f;

    //DAYTIME AMBIENT SOUND
    public AudioClip ambientMorning;
    private float volumehiM = 0.7f;
    private float volumeloM = 0f;
    private float volumeindM = 0f;

    private bool isMorningAbient = false;

    public float percentThroughDay;
    public Material dayToSunset;
    public Material sunsetToNight;
    public Material nightToSunrise;
    public Material sunriseToDay;
    public Color daylightColor = new Color(1, 0.9568f, 0.8392f, 1);
    public Color sunsetColor = new Color(1, 0.9682f, 0.6462f, 1);
    public Color nightColor = new Color(0.946f, 0.929f, 1, 1);
    public Color sunriseColor = new Color(1, 0.9682f, 0.6462f, 1);
    public Light sunLight;
    //public Light bounceLight;
    public int secondsPerCycle;
    private float cycleStartTime = 0;

    private Vector3 sunDefaultPositionVector = new Vector3(-10, 0, -135);

    private static int nightTime = 10;
    private static int dayTime = 40;
    private static int transitionTime = (100 - (nightTime + dayTime)) / 4;
    private static Quaternion sunStartAngle = Quaternion.Euler(0, 190, 0);
    private static Vector3 axisOfSunRotation = new Vector3(5, -1, 2);

    // Use this for initialization
    void Start()
    {
        cycleStartTime = -1 * (secondsPerCycle / 4);
        percentThroughDay = 25; // starts with daylight
        applyChanges();

        ambientSound = GetComponent<AudioSource>();
        ambientSound.clip = ambientMorning;
        ambientSound.loop = true;
        ambientSound.volume = 0;
        ambientSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        /* MANUAL TIME SHIFT: FOR DEBUGGING
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
        } */
        if (Time.time - cycleStartTime >= secondsPerCycle) {
            cycleStartTime = Time.time;
        }
        percentThroughDay = Mathf.Lerp(0, 100, (Time.time - cycleStartTime) / secondsPerCycle);

        applyChanges();


        //Adjust volume of daytime sound
        if(percentThroughDay >= 0 && percentThroughDay < 50) {
          if(!isMorningAbient){
            ambientSound.clip = ambientMorning;
            ambientSound.Play();
            isMorningAbient = true;
            print("Sound changed to morning ambient");
          }
          volumeindM = 1 - (float)((Mathf.Abs(25 - percentThroughDay)) / 25);
          ambientSound.volume = Mathf.Lerp(volumeloM, volumehiM, volumeindM);
        }
        //Adjust volume of nighttime sound
        else if(percentThroughDay >= 50 && percentThroughDay < 100) {
          if(isMorningAbient){
            ambientSound.clip = ambientNight;
            ambientSound.Play();
            isMorningAbient = false;
            print("Sound changed to night ambient");
          }
          volumeindN = 1 - (float)((Mathf.Abs(75 - percentThroughDay)) / 25);
          ambientSound.volume = Mathf.Lerp(volumeloN, volumehiN, volumeindN);
        }
    }

    public void applyChanges()
    {
        sunLight.transform.rotation = sunStartAngle * Quaternion.AngleAxis(Mathf.Lerp(0, 360, percentThroughDay/100.0f), axisOfSunRotation);
        if (percentThroughDay <= transitionTime)
        // Night to Sunrise
        {
            float percentThroughPhase = percentThroughDay / transitionTime;
            RenderSettings.skybox = nightToSunrise;
            nightToSunrise.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(0, 1, percentThroughPhase);
            sunLight.color = sunriseColor;
        }
        else if (percentThroughDay > transitionTime && percentThroughDay <= (2 * transitionTime))
        // Sunrise to Day
        {
            float percentThroughPhase = (percentThroughDay - transitionTime) / transitionTime;
            RenderSettings.skybox = sunriseToDay;
            sunriseToDay.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(1, 1.5f, percentThroughPhase);
            Color lightColor = Color.Lerp(sunriseColor, daylightColor, percentThroughPhase);
            sunLight.color = lightColor;
        }
        else if (percentThroughDay > (2 * transitionTime) && percentThroughDay <= (2 * transitionTime) + dayTime)
        // Day
        {
            float percentThroughPhase = (percentThroughDay - (2 * transitionTime)) / dayTime;
            RenderSettings.skybox = sunriseToDay;
            sunriseToDay.SetFloat("_Blend", 1);
            sunLight.intensity = 1.5f;
            sunLight.color = daylightColor;
        }
        else if (percentThroughDay > ((2 * transitionTime) + dayTime) && percentThroughDay <= (3 * transitionTime) + dayTime)
        // Day to Sunset
        {
            float percentThroughPhase = (percentThroughDay - (2 * transitionTime) - dayTime) / transitionTime;
            RenderSettings.skybox = dayToSunset;
            dayToSunset.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(1.5f, 1, percentThroughPhase);
            Color lightColor = Color.Lerp(daylightColor, sunsetColor, percentThroughPhase);
            sunLight.color = lightColor;

        }
        else if (percentThroughDay > ((3 * transitionTime) + dayTime) && percentThroughDay <= (4 * transitionTime) + dayTime)
        // Sunset to Night
        {
            float percentThroughPhase = (percentThroughDay - (3 * transitionTime) - dayTime) / transitionTime;
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(1, 0, percentThroughPhase);
            sunLight.color = sunsetColor;
        }
        else
        // Night
        {
            float percentThroughPhase = (percentThroughDay - (4 * transitionTime) - dayTime) / nightTime;
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", 1);
            sunLight.intensity = 0;
            sunLight.color = sunsetColor;
        }
    }

    private void OnApplicationQuit()
    {
        //reset material properties so they aren't set permanently
        percentThroughDay = 25;
        applyChanges();
    }
}
