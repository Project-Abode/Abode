using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.SportShooting {
	public class EnterGame : MonoBehaviour {
		private void Update()
		{
			// if (Input.GetKeyDown(KeyCode.W))
			// {
			// 	StartGame("World");
			// }

			// if(Input.GetKeyDown(KeyCode.L)) {
			// 	StartGame("LevelStreaming");
			// }

			// if(Input.GetKeyDown(KeyCode.E)) {
			// 	StartGame("Elevator");
			// }

			//In lobby
			// if(Input.GetKeyDown(KeyCode.Space)) {
			// 	StartGame("Menu");
			// }

		}

		void Awake(){
			StartGame("Menu");
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

			if(setting.isHost) {
				//sync
				setting.OnHostRequstedSync();

			}else {
				setting.CopyBufferIntoSettings();
			}

			StartGame(worldDict[setting.method]);
		}

		public void StartGame(string roomName)
		{
			GameController.Instance.StartGame(roomName);
		}

	}
}