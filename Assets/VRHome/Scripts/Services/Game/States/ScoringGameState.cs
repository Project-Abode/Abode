using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class ScoringGameState : BaseGameState
    {
        bool _isActive = false;
        float _stateLifeTime = 0;

        public override void InitState()
        {
            Debug.Log("Scoring State");
            base.InitState();

            NetworkController.OnRoundEnded += SomeoneWonMessage;
            NetworkController.OnRoundEnded += ActivateState;

            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }

            NetworkController.Instance.NotifyRoundEnded(GameModel.Instance.WinningPlayer);
        }

        public override void FinishState()
        {
            base.FinishState();

            NetworkController.OnRoundEnded -= SomeoneWonMessage;
            NetworkController.OnRoundEnded -= ActivateState;

            GameModel.Instance.ResetRoundData();
            NetworkController.Instance.NotifyToUpdateScore();
        }

        public override void ExecuteState()
        {
            base.ExecuteState();
            if (_isActive)
            {
                _stateLifeTime += Time.deltaTime;
            }

            if(_stateLifeTime > GameView.Instance.WinScreenTimeout)
            {
                GameModel.Instance.ChangeGameState(new PlayingGameState());
            }          
        }

        void SomeoneWonMessage(PhotonPlayer winningPlayer)
        {
            GameView.Instance.ShowGamePopupPanel("Round Ended!", "Player ID: " + winningPlayer.ID + " won the round!", GameView.Instance.WinScreenTimeout);
        }

        void ActivateState(PhotonPlayer winningPlayer)
        {
            _isActive = true;
        }
    }
}
