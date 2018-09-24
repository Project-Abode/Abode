using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ExitGames.SportShooting
{
    public class StatusPopupPresenter : MonoBehaviour
    {
        [SerializeField]
        Text _message;

        public void UpdateMessage(string message, float selfDestroyTime)
        {
            if(message != null)
            {
                _message.text = message;
            }

            Destroy(gameObject, selfDestroyTime);
        }
    }
}
