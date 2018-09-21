using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class AimingLaserSteamVR : AimingLaser
    {
        [SerializeField]
        SteamVR_TrackedController _trackedController;

        protected override void Awake()
        {
            base.Awake();
            _trackedController.PadClicked += ToggleLaser;
        }

        protected void OnDestroy()
        {
            _trackedController.PadClicked -= ToggleLaser;
        }

        private void ToggleLaser(object sender, ClickedEventArgs e)
        {
            ToggleLaser();
        }
    }
}