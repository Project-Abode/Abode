using UnityEngine;
using System.Collections;
using Photon;

namespace ExitGames.SportShooting
{
    public class Connection : PunBehaviour
    {

        public void Init()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.INITIALIZING);
            PhotonNetwork.autoJoinLobby = false;
        }

        public void Connect()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.CONNECTING_TO_SERVER);
            if (PhotonNetwork.connected)
            {
                OnConnectedToMaster();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(NetworkController.NETCODE_VERSION);
            }
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
            if (PhotonNetwork.inRoom)
            {
                OnJoinedRoom();
            }
            else
            {
                NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.CREATING_ROOM);
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);
        }

        public override void OnJoinedRoom()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.ROOM_JOINED);
        }

        public override void OnCreatedRoom()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.ROOM_CREATED);            
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.SOME_PLAYER_CONNECTED, newPlayer);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.SOME_PLAYER_DISCONNECTED, otherPlayer);
        }

        public override void OnDisconnectedFromPhoton()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.DISCONNECTED);
        }
        #endregion        
    }
}
