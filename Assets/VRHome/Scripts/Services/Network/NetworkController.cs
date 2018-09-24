using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
        }

        void OnEnable()
        {
            PhotonNetwork.OnEventCall += this.ProcessNetworkEvent;
        }

        void OnDisable()
        {
            PhotonNetwork.OnEventCall -= this.ProcessNetworkEvent;
        }

        public void StartMultiplayerGame()
        {
            _connection.Init();
            _connection.Connect();
        }

        public void EndMultiplayerGame()
        {
            _connection.Disconnect();
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
                    if (OnGameConnected != null)
                    {
                        OnGameConnected();
                    }
                    ChangeNetworkState(NetworkState.PLAYING);
                    break;
                case NetworkState.ROOM_JOINED:
                    if (OnGameConnected != null)
                    {
                        OnGameConnected();
                    }
                    NotifyToUpdateScore();
                    ChangeNetworkState(NetworkState.PLAYING);
                    break;
                case NetworkState.SOME_PLAYER_CONNECTED:
                    if(OnSomePlayerConnected != null)
                    {
                        OnSomePlayerConnected((PhotonPlayer)stateData);
                    }
                    ChangeNetworkState(NetworkState.PLAYING);
                    break;
                case NetworkState.SOME_PLAYER_DISCONNECTED:
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
    