using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ExitGames.SportShooting
{
    public class LaserPointer : MonoBehaviour
    {
        float _maxRayDistance = 100f;

        [SerializeField]
        LayerMask _hitLayer;       

        RaycastHit _hitInfo;
        LineRenderer _lineRenderer;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
        
        protected virtual void Update()
        {
            // Find collider which was hit by laser aiming
            if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, _maxRayDistance, _hitLayer))
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

        protected void ClickOnHitObject()
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, _maxRayDistance, _hitLayer))
            {
                Button button = _hitInfo.collider.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                }
            }
        }
    }
}
