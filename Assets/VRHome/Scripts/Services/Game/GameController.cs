using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExitGames.SportShooting
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        void Start()
        {
            Instance = this;
            StartGame("Menu");
        }

        void Update() {
            // if(Input.GetKeyDown(KeyCode.Space)) {
            //     StartGame("Menu");
            // }
        }

		List<string> worldDict = new List<string>{
			"World",//portal
			"MagicWand",
			"Elevator",
			"LevelStreaming",
			"MagicDoor",
			"HotAirballoon"
		};

		public void EnterGameWithSettings(){
			var setting = Settings.instance;

			// if(setting.isHost) {
			// 	//sync
			// 	setting.OnHostRequstedSync();
			// }else {
			// 	setting.CopyBufferIntoSettings();
			// }

			StartGame(worldDict[setting.method]);
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
            Debug.Log("Game Controller start game: " + id);
            GameModel.Instance.ChangeGameState(new ConnectingGameState());
            GameView.Instance.ShowNetworkPanel();
            NetworkController.Instance.StartMultiplayerGame(id);
        }


        // public void JoinRoom(string id)
        // {
        //     GameModel.Instance.ChangeGameState(new ConnectingGameState());
        //     GameView.Instance.ShowNetworkPanel();
        //     NetworkController.Instance.JoinRoom(id);
        // }

        // public void GoBackToHome() {
        //     GameModel.Instance.ChangeGameState(new ConnectingGameState());
        //     //NetworkController.Instance.JoinRoom(id);
        // }

    }
}
