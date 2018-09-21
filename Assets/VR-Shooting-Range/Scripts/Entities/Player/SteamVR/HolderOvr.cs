using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class HolderOvr : Holder
    {
        [SerializeField]
        Transform _holderTarget;
        [SerializeField]
        Transform _lookAtTarget;
        [SerializeField]
        bool _syncrhoizePosition = true;
        [SerializeField]
        bool _syncrhoizeRotation = true;

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

            if (_syncrhoizePosition)
            {
                transform.position = _holderTarget.position;
            }
            if (_syncrhoizeRotation)
            {
                transform.rotation = _holderTarget.rotation;
            }

            transform.LookAt(_lookAtTarget.position);
        }
    }
}
