using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ExitGames.SportShooting
{
    public class AimingLaser : MonoBehaviour
    {
        float _maxRayDistance = 100f;

        [SerializeField]
        LayerMask _hitLayer;

        RaycastHit _hitInfo;
        LineRenderer _lineRenderer;

        protected virtual void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();           
        }

        protected virtual void Update()
        {
            // Find collider which was hit by laser aiming
            if (_lineRenderer.enabled && Physics.Raycast(transform.position, transform.forward, out _hitInfo, _maxRayDistance, _hitLayer))
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _hitInfo.point);
            }
            else
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, transform.position + transform.forward * _maxRayDistance);
            }
        }

        protected void ToggleLaser()
        {
            _lineRenderer.enabled = !_lineRenderer.enabled;            
        }
    }
}
