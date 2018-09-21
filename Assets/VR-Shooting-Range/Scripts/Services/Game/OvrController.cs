using UnityEngine;
using System.Collections;

public class OvrController : MonoBehaviour
{
    [SerializeField]
    private GameObject _ovrServicesPrefab;

    private void Awake()
    {
        if (OVRManager.instance == null)
        {
            Instantiate<GameObject>(_ovrServicesPrefab);
        }
    }
}
