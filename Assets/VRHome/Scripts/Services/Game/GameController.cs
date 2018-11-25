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
            if(Input.GetKeyDown(KeyCode.X)) {
                StartGame("Menu");
            }
        }

		List<string> worldDict = new List<string>{
			// "HearthWorld",//portal + magic wand
			// "MagicWand",
			// "Elevator",
			// "LevelStreaming",
			// "MagicDoor",
			// "HotAirballoon",
            // "WandTest"
            
            "GardenWorld",
            "HearthWorld",
            "Elevator",
            "LevelStreaming",
            "WandTest"

		};

		public void EnterGameWithSettings(){
			var setting = Settings.instance;

            if(setting.method == 404) {
                StartGame("WandTest");
                return;
            }

            if(setting.method == 2 || setting.method == 3){
                StartGame(worldDict[setting.method]);
                return;
            }
            else {
                if(setting.method == 0 || setting.method == 1) {
                    StartGame(worldDict[1]); //Hearth
                }else {
                    StartGame(worldDict[0]); //Garden
                }

            }
		}

        // public void InitMainMenu()
        // {
        //     //GameModel.Instance.ChangeGameState(new MainMenuGameState());
        //     //GameView.Instance.ShowMainMenuPanel();
        // }

        // public void StartMultiplayerGame()
        // {
        //     //GameModel.Instance.ChangeGameState(new ConnectingGameState());
        //     //GameView.Instance.ShowNetworkPanel();
        //     //NetworkController.Instance.StartMultiplayerGame();
        // }


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
