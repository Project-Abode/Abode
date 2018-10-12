using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class LaserPointerSteamVR1 : LaserPointer
    {
        [SerializeField]
        SteamVR_TrackedController _trackedController;
    
        // public void OpenLaser() {
        //     //_trackedController = controller;
        //     _trackedController.PadClicked += ClickOnHitObject;

        // }

        // public void CloseLaser() {
        //     _trackedController.PadClicked -= ClickOnHitObject;
        //     //_trackedController = null;

        // }

        void OnEnable()
        {
            Debug.Log("Laser on Enable");
            _trackedController.TriggerClicked += ClickOnHitObject;
        }

        void OnDisable()    
        {
            Debug.Log("Laser on Disable");
            _trackedController.TriggerClicked -= ClickOnHitObject;
        }


        private void ClickOnHitObject(object sender, ClickedEventArgs e)
        {
            ClickOnHitObject();
        }
    }
}
