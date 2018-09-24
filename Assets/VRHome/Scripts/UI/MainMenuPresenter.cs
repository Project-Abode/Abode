using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class MainMenuPresenter : MonoBehaviour
    {
        public void OnStartButtonClick()
        {
            GameController.Instance.StartMultiplayerGame();
        }
    }
}
