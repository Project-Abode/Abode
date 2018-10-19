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
                //Hack
                var col = portal.gameObject.GetComponent<BoxCollider>();
                col.enabled = true;

                //Hack end
                portal.ShowEntity();

                //var socket = GameObject.Find("socket").GetComponent<SocketClient>();
                //socket.SendMyMessage("accept invitation");

            }
            // else {
            //     var col = portal.gameObject.GetComponent<BoxCollider>();
            //     col.enabled = false;
            //     portal.DisappearEntity();
            // }
            
        }
    }
}
