using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class AimingLaserGoogleVR : AimingLaser
    {
        protected override void Awake()
        {
            base.Awake();
        } 
        
        protected override void Update()
        {
            base.Update();
#if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
            if (GvrController.AppButtonDown)
            {
                ToggleLaser();
            }
#endif
        }       
    }
}
