using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class LaserPointerSteamVR : LaserPointer
    {
        [SerializeField]
        SteamVR_TrackedController _trackedController;
        
        void OnEnable()
        {
            _trackedController.TriggerClicked += ClickOnHitObject;
        }

        void OnDisable()
        {
            _trackedController.TriggerClicked -= ClickOnHitObject;
        }

        private void ClickOnHitObject(object sender, ClickedEventArgs e)
        {
            ClickOnHitObject();
        }
    }
}
