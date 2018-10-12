using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class MainMenuPresenter : MonoBehaviour
    {
        public GameObject portal;
        public void OnStartButtonClick()
        {
            GameController.Instance.StartMultiplayerGame();
        }

        public void OnGoButtonClick() {
            portal.SetActive(true);
        }
    }
}
