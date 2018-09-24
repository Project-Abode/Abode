using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class HolderGoogleVR : Holder
    {
        void Update()
        {
            #if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
            Quaternion ori = GvrController.Orientation;
            transform.rotation = ori;           
            #endif
        }
    }
}
