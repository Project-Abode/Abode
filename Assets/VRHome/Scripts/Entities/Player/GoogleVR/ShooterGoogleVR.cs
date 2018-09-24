using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class ShooterGoogleVR : Shooter
    {
        public void Update()
        {
            #if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
            if (_photonView != null && _photonView.isMine && GvrController.TouchDown)
            {
                ShootAttempt();
            }
            #endif
        }
    }
}
