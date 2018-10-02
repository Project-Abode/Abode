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
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.automaticallySyncScene = true;
        }

        public void Connect() // connect to server
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.CONNECTING_TO_SERVER);
            if (PhotonNetwork.connected) //already connect
            {
                OnConnectedToMaster();
            }
            else // not connected
            {
                PhotonNetwork.ConnectUsingSettings(NetworkController.NETCODE_VERSION);
            }
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void JoinRoom(string id)
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
            PhotonNetwork.JoinRoom(id);
        }

        public void LeaveRoom() {
            Debug.Log("[Connection]: Leave Room");
            PhotonNetwork.LeaveRoom();
        }

        public void CreateAndJoinMyRoom() {
            Debug.Log("[Connection]Create Room:" + NetworkController.myID);
            //!!!
            PhotonNetwork.JoinOrCreateRoom(NetworkController.myID , new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);
        }

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {

            // Here we only create our own room
            CreateAndJoinMyRoom();
            // Debug.Log("[Connection]Create Room:" + NetworkController.myID);
            // PhotonNetwork.CreateRoom(NetworkController.myID , new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);


            //Reference:
            //NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
            //if (PhotonNetwork.inRoom)
            //{
            //    OnJoinedRoom();
            //}
            //else
            //{
            //    NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
            //    PhotonNetwork.JoinRandomRoom();
            //}
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            //NetworkController.Instance.ChangeNetworkState(NetworkState.CREATING_ROOM);
            //PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            //NetworkController.Instance.ChangeNetworkState(NetworkState.CREATING_ROOM);
            //PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);

            Debug.Log("[Connection]Failed to join room");
            Debug.Log(codeAndMsg[1]);
            
            //Faile to join a room, GO back to my home
            NetworkController.toID = NetworkController.myID;
            NetworkController.Instance.LoadToGoScene();
            CreateAndJoinMyRoom();

        }

        public override void OnJoinedRoom()
        {
            Debug.Log("[Connection]Join room Successful");
            NetworkController.Instance.ChangeNetworkState(NetworkState.ROOM_JOINED);
        }

        public override void OnLeftRoom() {
            Debug.Log("[Connection]Left room Successful");
            NetworkController.Instance.LoadToGoScene();
        }

        public override void OnJoinedLobby() {
            JoinRoom(NetworkController.toID);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("[Connection]Create Room Success");
            NetworkController.Instance.ChangeNetworkState(NetworkState.ROOM_CREATED);            
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {

            Debug.Log("[Connection]Create Room Fail");
            //TODO:
            //1:user already exist
            //2:other error


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
