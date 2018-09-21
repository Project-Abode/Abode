using UnityEngine;
using System.Collections;

public class TrackerGoogleVR : MonoBehaviour {
    [SerializeField]
    Transform _target;

    [SerializeField]
    bool _trackPosition;
    [SerializeField]
    bool _trackRotation;
    void Update()
    {
        #if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
        if (_target == null)
        {
            return;
        }

        if (_trackPosition)
        {
            transform.position = _target.position;
        }

        if (_trackRotation)
        {
            Quaternion ori = GvrController.Orientation;
            transform.rotation = ori;
        }
        #endif        
    }
}
