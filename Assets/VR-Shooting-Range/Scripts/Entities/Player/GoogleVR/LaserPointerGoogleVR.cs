using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class LaserPointerGoogleVR : LaserPointer
    {
        protected override void Update()
        {
            base.Update();
            #if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
            if (GvrController.ClickButtonDown)
            {
                ClickOnHitObject();
            }
            #endif
        }
    }
}
