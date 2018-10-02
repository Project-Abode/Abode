using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.SportShooting
{
    public class LobbyMenuPresenter : MonoBehaviour
    {

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartPlayer("001");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartPlayer("002");
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                JoinRoom("001");
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                JoinRoom("002");
            }


        }

        public void StartPlayer(string id)
        {
            //GameController.Instance.StartMultiplayerGame();
            //Debug.Log("Click");
            GameController.Instance.StartGame(id);
        }


        public void JoinRoom(string id) {
            GameController.Instance.JoinRoom(id);
        }

    }

}
