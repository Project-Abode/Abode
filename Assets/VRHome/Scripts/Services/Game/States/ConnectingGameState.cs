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

        void InitGame() // INIT: guest or host
        {
            SetPlayerData();
            GameModel.Instance.ChangeGameState(new InitializingGameState());
        }

        void SetPlayerData()
        {

            //
            // Position: 0 - host, 1 - guest

            // List<int> freePositions = new List<int>();
            // for(int pos = 0; pos < NetworkController.MAX_PLAYERS; pos++)
            // {
            //     freePositions.Add(pos); //[0,1,2,3,4]
            // } // all position

            Debug.Log("Print Players in network");
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                // if(player.CustomProperties["position"] != null)
                // {
                //     freePositions.Remove((int)player.CustomProperties["position"]);
                // }

                Debug.Log(player.ID);

            } // remove already taken position
            Debug.Log("Print Players in network - end");

            // string playerName = string.Empty;

            // switch (freePositions[0])
            // {
            //     case 0:
            //         playerName = "Player RED";
            //         break;
            //     case 1:
            //         playerName = "Player BLUE";
            //         break;
            //     case 2:
            //         playerName = "Player YELLOW";
            //         break;
            //     case 3:
            //         playerName = "Player GREEN";
            //         break;
            //     case 4:
            //         playerName = "Player BLACK";
            //         break;
            // }

            int spawn_pos = PhotonNetwork.playerList.Length > 1 ? 1 : 0;

            Hashtable playerInfo = new Hashtable();
            playerInfo.Add("position", spawn_pos);
            playerInfo.Add("roundScore", 0);
            //playerInfo.Add("name", playerName);
            playerInfo.Add("name", "NULL");
            PhotonNetwork.player.SetCustomProperties(playerInfo);

            Debug.Log("Spawn Point:" + PhotonNetwork.player.CustomProperties["position"]);
            Debug.Log("ID:"+ PhotonNetwork.player.ID);
        }
    }
}
