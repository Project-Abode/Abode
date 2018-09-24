using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class PlayingGameState : BaseGameState
    {
        float _lastTrapSpawnTime = 0f;

        public override void InitState()
        {
            base.InitState();

            NetworkController.OnSomePlayerConnected += SomeoneConnectedMessage;
            NetworkController.OnSomePlayerDisconnected += SomeoneDisconnectedMessage;
            NetworkController.OnPlayerTrapHit += ShowTrapHitMessage;
        }

        public override void FinishState()
        {
            base.FinishState();

            NetworkController.OnSomePlayerConnected -= SomeoneConnectedMessage;
            NetworkController.OnSomePlayerDisconnected -= SomeoneDisconnectedMessage;
            NetworkController.OnPlayerTrapHit -= ShowTrapHitMessage;
        }

        public override void ExecuteState()
        {
            base.ExecuteState();

            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }            
            
            //Miao: comment out
            //AttemptSpawnTrap();
        }

        void AttemptSpawnTrap()
        {
            if (_lastTrapSpawnTime < 0f)
            {
                if(GameModel.Instance.IsRoundEnded)
                {
                    NetworkController.Instance.NotifyChangeToScoringState();
                    GameModel.Instance.ChangeGameState(new ScoringGameState());
                } else
                {
                    GameModel.Instance.BuildTrap();
                    _lastTrapSpawnTime = GameModel.Instance.TrapSpawnInterval;
                }
            }
            _lastTrapSpawnTime -= Time.deltaTime;
        }

        void SomeoneConnectedMessage(PhotonPlayer somePlayer)
        {
            GameView.Instance.ShowGamePopupPanel("NEW PLAYER", "Player ID: " + somePlayer.ID + " connected");
        }

        void SomeoneDisconnectedMessage(PhotonPlayer somePlayer)
        {
            GameView.Instance.ShowGamePopupPanel("PLAYER DISCONNECTED", "Player ID: " + somePlayer.ID + " disconnected");
        }

        void ShowTrapHitMessage(int playerID)
        {
            PhotonPlayer[] players = PhotonNetwork.playerList;
            string playerName = playerID.ToString();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].ID == playerID)
                {
                    playerName = players[i].CustomProperties["name"].ToString();
                    break;
                }
            }

            GameView.Instance.ShowStatusPopupPanel("Player: '" + playerName + "' have hit the trap");
        }
    }
}
