using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFlicker : MonoBehaviour {

    public float flickerProbability;
    public float minIntensity;
    public float maxIntensity;
    public float minRange;
    public float maxRange;
    public float maxMovement;
    public float maxShadowStrength;
    public float minShadowStrength;
    public float transitionTime;
    public Light flameLight;
    public bool returnToMedian;

    private bool isFlickering;
    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        originalPosition = flameLight.gameObject.transform.position;

        flameLight.intensity = (minIntensity + maxIntensity) / 2;
        flameLight.range = (minRange + maxRange) / 2;
        flameLight.shadowStrength = (minShadowStrength + maxShadowStrength) / 2;
	}
 
    void Update()
    {
        float likelihood = Random.Range(1, 100);
        if (likelihood < flickerProbability && !isFlickering){
            StartCoroutine(IntensityShift(Random.Range(minIntensity, maxIntensity)));
            StartCoroutine(RangeShift(Random.Range(minRange, maxRange)));
            StartCoroutine(ShadowShift(Random.Range(minShadowStrength, maxShadowStrength)));
            StartCoroutine(PositionShift());
            isFlickering = true;
        }
    }

    private IEnumerator IntensityShift(float newIntensity)
    {
        float oldValue = flameLight.intensity;
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            flameLight.intensity = Mathf.Lerp(oldValue, newIntensity, t / transitionTime);
            yield return new WaitForEndOfFrame();
        }

        if(returnToMedian)
        {
            for (float i = 0; i < transitionTime; i += Time.deltaTime)
            {
                flameLight.intensity = Mathf.Lerp(newIntensity, oldValue, i / transitionTime);
                yield return new WaitForEndOfFrame();
            }
        }
        isFlickering = false;
    }

    private IEnumerator RangeShift(float newRange)
    {
        float oldValue = flameLight.range;
        for (float t = 0; t < transitionTime; t+= Time.deltaTime)
        {
            flameLight.range = Mathf.Lerp(oldValue, newRange, t / transitionTime);
            yield return new WaitForEndOfFrame();
        }

        if (returnToMedian)
        {
            for (float i = 0; i < transitionTime; i += Time.deltaTime)
            {
                flameLight.range = Mathf.Lerp(newRange, oldValue, i / transitionTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator ShadowShift(float newStrength)
    {
        float oldValue = flameLight.shadowStrength;
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            flameLight.shadowStrength = Mathf.Lerp(oldValue, newStrength, t / transitionTime);
            yield return new WaitForEndOfFrame();
        }

        if (returnToMedian)
        {
            for (float i = 0; i < transitionTime; i += Time.deltaTime)
            {
                flameLight.shadowStrength = Mathf.Lerp(newStrength, oldValue, i / transitionTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator PositionShift()
    {
        float oldX = flameLight.gameObject.transform.position.x;
        float oldY = flameLight.gameObject.transform.position.y;
        float oldZ = flameLight.gameObject.transform.position.z;

        float newX = Random.Range(-1 * maxMovement, maxMovement) + originalPosition.x;
        float newY = Random.Range(-1 * maxMovement, maxMovement) + originalPosition.y;
        float newZ = Random.Range(-1 * maxMovement, maxMovement) + originalPosition.z;

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            float changeX = Mathf.Lerp(oldX, newX, t / transitionTime);
            float changeY = Mathf.Lerp(oldY, newY, t / transitionTime);
            float changeZ = Mathf.Lerp(oldZ, newZ, t / transitionTime);
            flameLight.gameObject.transform.position = new Vector3(changeX, changeY, changeZ);
            yield return new WaitForEndOfFrame();
        }
    }
}
