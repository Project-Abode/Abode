using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCanvasTrigger : MonoBehaviour
{
    public FrameManager myManager;

    void OnTriggerStay(Collider other)
    {
        myManager.mayHaveFoundController(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        myManager.mayHaveLostController(other.gameObject);
    }
}
