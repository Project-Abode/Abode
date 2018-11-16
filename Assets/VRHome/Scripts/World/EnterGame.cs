using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.SportShooting {
	public class EnterGame : MonoBehaviour {
        private bool go=false;
		private void Update()
		{
		
			if (Input.GetKeyDown(KeyCode.W)|| go)
			{
                go = false;
                StartPlayer("World");
                
			}            

			if(Input.GetKeyDown(KeyCode.D)) {
				StartPlayer("LevelStreamingTest");
			}

		}

		public void StartPlayer(string id)
		{
			GameController.Instance.StartGame(id);
		}

        public void GoToWorld()
        {
            go = true;
        }
	}
}