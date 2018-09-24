using UnityEngine;
using System.Collections;

public class OvrCenterEyeTarget : MonoBehaviour {

    [SerializeField]
    private GameObject _centerEyeAnchor;
    private float _maxDistance = 20f;
    // Update is called once per frame
    void Update () {
        transform.position = _centerEyeAnchor.transform.position + _centerEyeAnchor.transform.forward * _maxDistance;
    }
}
