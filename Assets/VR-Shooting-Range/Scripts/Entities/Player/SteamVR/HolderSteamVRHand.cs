using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class HolderSteamVRHand : Holder
    {
        [SerializeField]
        Transform _holderTarget;
        

        protected override void Awake()
        {
            base.Awake();
            if (_holderTarget == null)
            {
                Destroy(this);
            }
        }

        void Update()
        {
            //Syncrhoize transform with target transform
            if (_holderTarget == null)
            {
                return;
            }

            transform.position = _holderTarget.position;
            transform.rotation = _holderTarget.rotation;

        }
    }
}
