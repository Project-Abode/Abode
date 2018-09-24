using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bowl : MonoBehaviour {

    public float transitionTime = 1.0f; //seconds

    public int maxObjects;

    protected int numObjects = 0;
    
    public void AddObject()
    {
        numObjects++;
        Refresh();
    }

    public void RemoveObject()
    {
        numObjects--;
        Refresh();
    }

    private void Refresh()
    {
        StartCoroutine(TransitionToValue(((float)numObjects) / maxObjects));
    }

    private IEnumerator TransitionToValue(float newValue)
    {
        float oldValue = GetValue();
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            SetValue(Mathf.Lerp(oldValue, newValue, t / transitionTime));
            yield return new WaitForEndOfFrame();
        }
        SetValue(newValue);
    }

    protected abstract float GetValue(); //between 0 and 1

    protected abstract void SetValue(float newValue); //between 0 and 1
}
