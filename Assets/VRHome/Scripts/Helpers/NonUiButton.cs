using UnityEngine;

namespace ExitGames.SportShooting
{
    public class NonUiButton : MonoBehaviour
    {
        public void EndMatch()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogWarning("You can not end the match because you are not the MasterClient.");
            }
            
            GameModel.Instance.ChangeGameState(new ScoringGameState());
        }

        public void LeaveMatch()
        {
            NetworkController.Instance.EndMultiplayerGame();
            GameController.Instance.InitMainMenu();            

            PhotonNetwork.player.CustomProperties.Clear();
        }
    }
}