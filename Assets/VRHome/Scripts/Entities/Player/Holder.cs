using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    [RequireComponent(typeof(PhotonView))]
    public class Holder : MonoBehaviour
    {      
        protected virtual  void Awake()
        {
            var photonView = GetComponent<PhotonView>();
            if (!photonView.isMine)
            {
                Destroy(this);
            }
        }
    }
}
