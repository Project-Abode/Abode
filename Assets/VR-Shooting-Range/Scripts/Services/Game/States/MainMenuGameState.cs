using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class MainMenuGameState : BaseGameState
    {
        public override void InitState()
        {
            base.InitState();

            GameModel.Instance.BuildPlayer();

            foreach (SpawnPoint s in GameObject.FindObjectsOfType<SpawnPoint>())
            {
                s.RestoreDefaults();
            }
        }
    }
}
