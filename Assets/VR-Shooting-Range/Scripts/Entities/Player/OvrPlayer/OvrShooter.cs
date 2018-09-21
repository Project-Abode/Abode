using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class OvrShooter : MonoBehaviour
    {
        [SerializeField]
        LineRenderer _bulletTrace;

        [SerializeField]
        Transform _tip;

        [SerializeField]
        GameObject _gunFire;

        [SerializeField]
        LayerMask _hitLayer;

        [SerializeField]
        float _maxBulletDistance;

        [SerializeField]
        OVRCameraRig _cameraRig;

        [SerializeField]
        PhotonView _photonView;

        [SerializeField]
        NetItemsSynchronizer _synchronizer;

        RaycastHit _hitInfo;

        private AudioSource _shotSound;

        void Awake()
        {
            _shotSound = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (_photonView.isMine && OVRInput.GetDown(OVRInput.Button.One))
            {
                ShootAttempt(null, new ClickedEventArgs());
            }
        }

        void ShootAttempt(object sender, ClickedEventArgs e)
        {
            _gunFire.SetActive(false);
            _synchronizer.ForceUpdate();
            _bulletTrace.enabled = true;

            //Check if we've hit the target
            if (Physics.Raycast(_tip.position, _tip.forward, out _hitInfo, _maxBulletDistance, _hitLayer))
            {
                _bulletTrace.SetPosition(0, _tip.position);
                _bulletTrace.SetPosition(1, _hitInfo.point);

                var hittedObject = _hitInfo.collider.GetComponent<Distructable>();
                if (hittedObject != null)
                {
                    hittedObject.MarkToDestroy();
                }

                var b = _hitInfo.collider.GetComponent<NonUiButton>();
                if (b != null)
                {
                    if (b.CompareTag("EndMatchButton"))
                    {
                        b.EndMatch();
                    }
                    else if (b.CompareTag("LogoutButton"))
                    {
                        b.LeaveMatch();
                    }

                    _bulletTrace.enabled = false;
                    return;
                }
            }
            else
            {
                _bulletTrace.SetPosition(0, _tip.position);
                _bulletTrace.SetPosition(1, _tip.position + _tip.forward * _maxBulletDistance);
            }

            _gunFire.SetActive(true);
            _photonView.RPC("PlayShotSound", PhotonTargets.All);
            _synchronizer.ForceUpdate();
            StartCoroutine(DisableBulletTrace());
        }

        // Disable bullet trace with short timeout to create visual effect
        IEnumerator DisableBulletTrace()
        {
            yield return null;

            _bulletTrace.enabled = false;
        }

        [PunRPC]
        public void PlayShotSound()
        {
            _shotSound.PlayOneShot(_shotSound.clip);
        }
    }
}
