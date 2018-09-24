using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ExitGames.SportShooting
{
    public class GamePopupPresenter : MonoBehaviour
    {
        [SerializeField]
        Text _message;
        [SerializeField]
        Text _subMessage;

        public void UpdateMessage(string message, string subMessage, float selfDestroyTime)
        {
            if(message != null)
            {
                _message.text = message;
            }

            if(subMessage != null)
            {
                _subMessage.text = subMessage;
            }

            Destroy(gameObject, selfDestroyTime);
        }
    }
}
