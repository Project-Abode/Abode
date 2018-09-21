using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class OvrAimingLaser : MonoBehaviour
    {
        float _maxRayDistance = 100f;
        float _cursorDistance = 20f;

        [SerializeField]
        LayerMask _hitLayer;

        RaycastHit _hitInfo;
        LineRenderer _lineRenderer;

        [SerializeField]
        GameObject _cursorPrefab;

        [SerializeField]
        PhotonView _photonView;

        private GameObject _cursor;
        private Material _cursorMaterial;
        private float _localScale = 0.3f;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            if (_cursor == null && _photonView.isMine) {
                _cursor = Instantiate<GameObject>(_cursorPrefab);
                _cursor.transform.localScale = Vector3.one * _localScale;
                _cursorMaterial = _cursor.GetComponent<MeshRenderer>().material;
            }
        }

        void OnEnable()
        {
            if (_cursor != null)
            {
                _cursor.SetActive(true);
            }
        }

        void OnDisable()
        {
            if (_cursor != null)
            {
                _cursor.SetActive(false);
            }
        }

        void OnDestroy()
        {
            Destroy(_cursor);
            _cursor = null;
        }

        void Update()
        {
            if (!_lineRenderer.enabled) {
                return;
            }

            if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, _maxRayDistance, _hitLayer)) {
                UpdateCursor(Color.red);
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _hitInfo.point);
            }
            else {
                UpdateCursor(Color.white);
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, transform.position + transform.forward * _cursorDistance);
            }
        }

        void ToggleLaser(object sender, ClickedEventArgs e)
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }

        void UpdateCursor(Color color)
        {
            if (_cursor != null) { 
                _cursor.transform.rotation = transform.rotation;
                _cursor.transform.position = transform.position + transform.forward * _cursorDistance;
                _cursorMaterial.SetColor("_Color", color);
            }
        }
    }
}
