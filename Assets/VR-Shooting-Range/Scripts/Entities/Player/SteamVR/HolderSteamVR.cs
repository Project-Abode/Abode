using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class HolderSteamVR : Holder
    {
        [SerializeField]
        Transform _holderTarget;
        [SerializeField]
        Transform _lookAtTarget;

        protected override void Awake()
        {
            base.Awake();
            if (_holderTarget == null || _lookAtTarget == null)
            {
                Destroy(this);
            }
        }

        void Update()
        {
            //Syncrhoize transform with target transform
            if (_holderTarget == null || _lookAtTarget == null)
            {
                return;
            }

            transform.position = _holderTarget.position;
            transform.rotation = _holderTarget.rotation;
            transform.LookAt(_lookAtTarget.position);
        }
    }
}
