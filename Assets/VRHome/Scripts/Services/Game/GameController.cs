using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            //InitMainMenu();
        }

        public void InitMainMenu()
        {
            //GameModel.Instance.ChangeGameState(new MainMenuGameState());
            //GameView.Instance.ShowMainMenuPanel();
        }

        public void StartMultiplayerGame()
        {
            //GameModel.Instance.ChangeGameState(new ConnectingGameState());
            //GameView.Instance.ShowNetworkPanel();
            //NetworkController.Instance.StartMultiplayerGame();
        }


        public void StartGame(string id)
        {
            GameModel.Instance.ChangeGameState(new ConnectingGameState());
            GameView.Instance.ShowNetworkPanel();
            NetworkController.Instance.StartMultiplayerGame(id);
        }


        public void JoinRoom(string id)
        {
            GameModel.Instance.ChangeGameState(new ConnectingGameState());
            GameView.Instance.ShowNetworkPanel();
            NetworkController.Instance.JoinRoom(id);
        }

        public void GoBackToHome() {
            GameModel.Instance.ChangeGameState(new ConnectingGameState());
            //NetworkController.Instance.JoinRoom(id);
        }



    }
}
