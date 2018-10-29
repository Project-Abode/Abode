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

		}

		public void StartPlayer(string id)
		{
			GameController.Instance.StartGame(id);
		}
		
	}
}