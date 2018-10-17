using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class MainMenuPresenter : MonoBehaviour
    {
        //public GameObject portal;
        public Portal portal;
        public void OnStartButtonClick()
        {
            GameController.Instance.StartMultiplayerGame();
        }

        public void OnGoButtonClick() {

            if(!portal.visible) {
                portal.ShowEntity();
            }else {
                portal.DisappearEntity();
            }
            //portal.SetActive(true);
        }
    }
}
