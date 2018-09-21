using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class InitializingGameState : BaseGameState
    {
        public override void InitState()
        {
            base.InitState();

            GameModel.Instance.BuildPlayer();
            SpawnUI();

            GameModel.Instance.ChangeGameState(new PlayingGameState());
        }

        void SpawnUI()
        {
            GameView.Instance.ShowGamePopupPanel("Welcome!");
            //GameView.Instance.ShowScoringPanel();
        }
    }
}
