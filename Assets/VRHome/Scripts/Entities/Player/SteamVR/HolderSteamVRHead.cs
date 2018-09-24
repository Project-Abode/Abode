using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class HolderSteamVRHead : Holder
    {
        [SerializeField]
        Transform _holderTarget;
        [SerializeField]
        GameObject _headModel;
        
        protected override void Awake()
        {
            base.Awake();
            if (_holderTarget == null)
            {
                Destroy(this);
            }

            var photonView = GetComponent<PhotonView>();
            if(photonView.isMine)
            {
                _headModel.SetActive(false);
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
