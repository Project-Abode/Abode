using UnityEngine;
using System.Collections;
namespace ExitGames.SportShooting
{
    public class ShooterSteamVR : Shooter
    {
        [SerializeField]
        SteamVR_TrackedController _trackedController;

        void OnEnable()
        {
            _trackedController.TriggerClicked += ShootAttempt;
        }

        void OnDisable()
        {
            _trackedController.TriggerClicked -= ShootAttempt;
        }

        protected void ShootAttempt(object sender, ClickedEventArgs e)
        {
            if (_photonView != null && _photonView.isMine)
            {
                ShootAttempt();
            }
        }
    }
}
