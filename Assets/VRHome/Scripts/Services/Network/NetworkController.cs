using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace ExitGames.SportShooting
{
    public enum NetworkState
    {
        INITIALIZING, CONNECTING_TO_SERVER, CREATING_ROOM, ROOM_CREATED, JOINING_ROOM,
        ROOM_JOINED, PLAYING, SOME_PLAYER_CONNECTED, SOME_PLAYER_DISCONNECTED, DISCONNECTED
    }

    public enum NetworkEvent
    {
        UPDATE_SCORE, TRAP_HIT, TO_SCORING_STATE, ROUND_ENDED
    }

    public class NetworkController : MonoBehaviour {

        public static string myID;
        public static string toID;

        public const string NETCODE_VERSION = "1.0";
        public const int MAX_PLAYERS = 5;

        public NetworkState ActiveState { get; private set; }

        Connection _connection;

        public static event Action<NetworkState> OnNetworkStateChange;
        public static event Action OnGameConnected;
        public static event Action OnUpdateScore;
        public static event Action<PhotonPlayer> OnSomePlayerConnected;
        public static event Action<PhotonPlayer> OnSomePlayerDisconnected;
        public static event Action<int> OnPlayerTrapHit;
        public static event Action<PhotonPlayer> OnRoundEnded;

        public static NetworkController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
            _connection = GetComponent<Connection>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("OnSceneLoaded: " + scene.name);
            //if(scene.name.Equals("Lobby")) return;

            //if(scene.name == myID) {
                //_connection.CreateAndJoinMyRoom();
            //}else {
                //_connection.JoinRoom(toID);
            //}

            if(PhotonNetwork.connected) {
                Debug.Log("scene loaded and connected");
                _connection.CreateOrJoinRoom(myID);

            }

        }


        void OnEnable()
        {
            PhotonNetwork.OnEventCall += this.ProcessNetworkEvent;
        }

        void OnDisable()
        {
            PhotonNetwork.OnEventCall -= this.ProcessNetworkEvent;
        }

        public void StartMultiplayerGame(string id)
        {

            Debug.Log("StartMultiplayerGame");
            myID = id;
            toID = myID; // no need

            //if(!PhotonNetwork.connected) {
            _connection.Init();
            _connection.Connect();

        }

        public void JoinRoom(string id)
        {
            Debug.Log("network controller Join room");
            myID = id; 
            toID = id; //No need 
            _connection.LeaveRoom();
        }

        public void EndMultiplayerGame()
        {
            _connection.Disconnect();
        }

        public void LoadToGoScene() {
            SceneManager.LoadScene(toID, LoadSceneMode.Single);
        }

        public void ChangeNetworkState(NetworkState newState, object stateData = null)
        {
            ActiveState = newState;

            if(OnNetworkStateChange != null)
            {
                OnNetworkStateChange(ActiveState);
            }

            switch (ActiveState)
            {
                case NetworkState.ROOM_CREATED:
                    Debug.Log("[Network] Room Created");
                    //if (OnGameConnected != null)
                    //{
                        //SceneManager.LoadScene(myID, LoadSceneMode.Single);
                        //OnGameConnected();
                    //}

                    //ChangeNetworkState(NetworkState.PLAYING);
                    break;
                case NetworkState.ROOM_JOINED:
                    Debug.Log("[Network] Room Joined");
                    if (OnGameConnected != null)
                    {
                        
                        OnGameConnected();
                    }

                    ChangeNetworkState(NetworkState.PLAYING);
                    break;
                case NetworkState.SOME_PLAYER_CONNECTED:
                    Debug.Log("[Network] Some Player Joined");
                    if(OnSomePlayerConnected != null)
                    {
                        OnSomePlayerConnected((PhotonPlayer)stateData);
                    }
                    
                    // if(PhotonNetwork.isMasterClient) {
                    //     PhotonNetwork.LoadLevel(myID);
                    // }
                    
                    ChangeNetworkState(NetworkState.PLAYING);
                    break;
                case NetworkState.SOME_PLAYER_DISCONNECTED:
                    Debug.Log("[Network] Room Joined");
                    if (OnSomePlayerDisconnected != null)
                    {
                        OnSomePlayerDisconnected((PhotonPlayer)stateData);
                    }
                    NotifyToUpdateScore();
                    ChangeNetworkState(NetworkState.PLAYING);
                    break;
            }

        }        

        public void NotifyToUpdateScore()
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.UPDATE_SCORE,
                eventContent: null,
                sendReliable: true,
                options: customOptions
            );
        }

        public void NotifyTrapHit(int playerID)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TRAP_HIT,
                eventContent: playerID,
                sendReliable: true,
                options: customOptions
            );
        }

        public void NotifyChangeToScoringState()
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.Others;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.TO_SCORING_STATE,
                eventContent: null,
                sendReliable: true,
                options: customOptions
            );
        }

        public void NotifyRoundEnded(PhotonPlayer winningPlayer)
        {
            RaiseEventOptions customOptions = new RaiseEventOptions();
            customOptions.Receivers = ReceiverGroup.All;

            PhotonNetwork.RaiseEvent(
                (byte)NetworkEvent.ROUND_ENDED,
                eventContent: winningPlayer,
                sendReliable: true,
                options: customOptions
            );
        }

        private void ProcessNetworkEvent(byte eventcode, object content, int senderid)
        {
            Debug.Log("network event: " + eventcode);
            NetworkEvent recievedNetworkEvent = (NetworkEvent)eventcode;

            switch(recievedNetworkEvent)
            {
                case NetworkEvent.UPDATE_SCORE:
                    if (OnUpdateScore != null)
                    {
                        OnUpdateScore();
                    }
                    break;
                case NetworkEvent.TRAP_HIT:
                    if(OnPlayerTrapHit != null)
                    {
                        OnPlayerTrapHit((int)content);
                    }
                    break;
                case NetworkEvent.TO_SCORING_STATE:
                    GameModel.Instance.ChangeGameState(new ScoringGameState());
                    break;
                case NetworkEvent.ROUND_ENDED:
                    if (OnRoundEnded != null)
                    {
                        OnRoundEnded((PhotonPlayer)content);
                    }
                    break;
            }
        }
    }
}
    