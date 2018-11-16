using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.SportShooting {
	public class EnterGame : MonoBehaviour {

		private void Update()
		{
		
			if (Input.GetKeyDown(KeyCode.W))
			{
				StartPlayer("World");
			}

			if(Input.GetKeyDown(KeyCode.S)) {
				//StartPlayer("LevelStreamingTest");
				StartPlayer("LevelStreaming");
			}

			if(Input.GetKeyDown(KeyCode.E)) {
				//StartPlayer("LevelStreamingTest");
				StartPlayer("Elevator");
			}


		}

		public void StartPlayer(string id)
		{
			GameController.Instance.StartGame(id);
		}
		
	}
}