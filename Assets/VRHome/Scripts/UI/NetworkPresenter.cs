using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ExitGames.SportShooting
{
    public class NetworkPresenter : MonoBehaviour
    {
        [SerializeField]
        Text _message;

        void OnEnable()
        {
            NetworkController.OnNetworkStateChange += UpdateMessage;
        }

        void OnDisable()
        {
            NetworkController.OnNetworkStateChange -= UpdateMessage;
        }

        void UpdateMessage(NetworkState state)
        {
            switch(state)
            {
                case NetworkState.INITIALIZING:
                    _message.text = "Initializing";
                    break;
                case NetworkState.CONNECTING_TO_SERVER:
                    _message.text = "Connecting to server";
                    break;
                case NetworkState.JOINING_ROOM:
                    _message.text = "Joining Room";
                    break;
                case NetworkState.CREATING_ROOM:
                    _message.text = "Creating room";
                    break;
                case NetworkState.PLAYING:
                    _message.text = "Playing";
                    break;
            }
        }
    }
}
