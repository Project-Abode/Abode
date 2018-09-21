using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour {

    [SerializeField]
    float _radius = 1f;
    public float radius
    {
        get { return _radius; }
    }

    [SerializeField]
    Vector3 _position = Vector3.zero;
    public Vector3 truePosition
    {
        get { return transform.position + transform.rotation * _position; }
    }

    [SerializeField]
    float _multiplier = 1f;
    public float multiplier
    {
        get { return _multiplier; }
    }

    Vector3 _velocity;
    public Vector3 velocity
    {
        get { return _velocity; }
    }

    Vector3 _prevPosition;

    bool _needsToRegister = false;

    private void OnEnable()
    {
        if (WaterColliderManager.Instance)
        {
            WaterColliderManager.Instance.Register(this);
            _needsToRegister = false;
        }
        else
        {
            _needsToRegister = true;
        }

        _prevPosition = truePosition;
    }

    private void OnDisable()
    {
        if (!_needsToRegister && WaterColliderManager.Instance)
        {
            WaterColliderManager.Instance.Unregister(this);
            _needsToRegister = false;
        }
    }

    private void Update()
    {
        if (_needsToRegister)
        {
            WaterColliderManager.Instance.Register(this);
            _needsToRegister = false;
        }
        
        _velocity = truePosition - _prevPosition;
        _prevPosition = truePosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(truePosition, _radius);
    }
}
