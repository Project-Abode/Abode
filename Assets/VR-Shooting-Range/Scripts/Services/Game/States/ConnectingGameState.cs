using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace ExitGames.SportShooting
{
    public class ConnectingGameState : BaseGameState
    {
        public override void InitState()
        {
            base.InitState();
            NetworkController.OnGameConnected += InitGame;
        }

        public override void FinishState()
        {
            base.FinishState();
            NetworkController.OnGameConnected -= InitGame;
        }

        void InitGame()
        {
            SetPlayerData();
            GameModel.Instance.ChangeGameState(new InitializingGameState());
        }

        void SetPlayerData()
        {
            List<int> freePositions = new List<int>();
            for(int pos = 0; pos < NetworkController.MAX_PLAYERS; pos++)
            {
                freePositions.Add(pos);
            }

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if(player.CustomProperties["position"] != null)
                {
                    freePositions.Remove((int)player.CustomProperties["position"]);
                }
            }

            string playerName = string.Empty;

            switch (freePositions[0])
            {
                case 0:
                    playerName = "Player RED";
                    break;
                case 1:
                    playerName = "Player BLUE";
                    break;
                case 2:
                    playerName = "Player YELLOW";
                    break;
                case 3:
                    playerName = "Player GREEN";
                    break;
                case 4:
                    playerName = "Player BLACK";
                    break;
            }

            Hashtable playerInfo = new Hashtable();
            playerInfo.Add("position", freePositions[0]);
            playerInfo.Add("roundScore", 0);
            playerInfo.Add("name", playerName);
            PhotonNetwork.player.SetCustomProperties(playerInfo);

            Debug.Log(PhotonNetwork.player.CustomProperties["position"]);
            Debug.Log(PhotonNetwork.player.ID);
        }
    }
}
