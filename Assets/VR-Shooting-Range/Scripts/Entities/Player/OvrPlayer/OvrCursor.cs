using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class OvrCursor : MonoBehaviour
    {
        float _maxRayDistance = 100f;
        float _cursoreDistance = 3f;

        [SerializeField]
        LayerMask _hitLayer;

        RaycastHit _hitInfo;

        [SerializeField]
        GameObject _cursorPrefab;

        [SerializeField]
        OVRCameraRig _cameraRig;

        private GameObject _cursor;
        private Material _material;

        void Awake()
        {
            if (_cursor == null) {
                _cursor = Instantiate<GameObject>(_cursorPrefab);
                _material = _cursor.GetComponent<MeshRenderer>().material;
            }
        }

        void OnEnable()
        {
            if (_cursor != null) {
                _cursor.SetActive(true);
            }
        }

        void OnDisable()
        {
            if (_cursor != null) {
                _cursor.SetActive(false);
            }
        }

        void OnDestroy()
        {
            if (_cursor != null) {
                Destroy(_cursor);
                _cursor = null;
            }
        }

        void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, _maxRayDistance, _hitLayer))
            {
                _material.SetColor("_Color", Color.red);
                _cursor.transform.position = transform.position + transform.forward * _cursoreDistance;
                _cursor.transform.rotation = transform.rotation;

            }
            else
            {
                _material.SetColor("_Color", Color.white);
                _cursor.transform.rotation = transform.rotation;
                _cursor.transform.position = transform.position + transform.forward * _cursoreDistance;
            }

            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                ClickOnHitObject();
            }

        }

        void ClickOnHitObject()
        {
            if (_hitInfo.collider != null)
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
