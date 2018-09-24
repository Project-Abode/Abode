using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class GameView : MonoBehaviour
    {
        [Header("Timeout Settings")]
        [SerializeField]
        private float _winScreenTimeout;
        public float WinScreenTimeout
        {
            get { return _winScreenTimeout; }   
        }

        [Header("Presenter Panels")]
        [SerializeField]
        GameObject _mainMenuPanel;
        [SerializeField]
        GameObject _networkPanel;
        [SerializeField]
        GameObject _scoringPanel;
        [SerializeField]
        GameObject _gamePopupPanel;
        [SerializeField]
        GameObject _statusPopupPanel;

        public Transform UIRoot { get; set; }
        public Transform SideUIRoot { get; set; }

        public static GameView Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void ShowMainMenuPanel()
        {
            if (UIRoot == null) return;

            ClearUIRoot();

            var panel = Instantiate(_mainMenuPanel, UIRoot.position, UIRoot.rotation) as GameObject;
            panel.transform.SetParent(UIRoot, false);
        }

        public void ShowNetworkPanel()
        {
            if (UIRoot == null) return;

            ClearUIRoot();

            var panel = Instantiate(_networkPanel, UIRoot.position, UIRoot.rotation) as GameObject;
            panel.transform.SetParent(UIRoot, false);
        }

        public void ShowScoringPanel()
        {
            if (SideUIRoot == null) return;

            var panel = Instantiate(_scoringPanel) as GameObject;
            panel.transform.SetParent(SideUIRoot, false);
        }

        /// <summary>
        /// Show popup windows during gameplay
        /// </summary>
        /// <param name="message">main message</param>
        /// <param name="subMessage">optional secondary message</param>
        /// <param name="selfDestroyTime">time after which windows will be disposed automatically</param>
        public void ShowGamePopupPanel(string message, string subMessage = "", float selfDestroyTime = 2f)
        {
            if (UIRoot == null) return;

            ClearUIRoot();

            var panel = Instantiate(_gamePopupPanel, UIRoot.position, UIRoot.rotation) as GameObject;
            panel.transform.SetParent(UIRoot, false);

            var presenter = panel.GetComponent<GamePopupPresenter>();
            presenter.UpdateMessage(message, subMessage, selfDestroyTime);
        }

        /// <summary>
        /// Show status bar notification during gameplay
        /// </summary>
        /// <param name="message">status message content</param>
        /// <param name="selfDestroyTime">time after which status bar will be disposed automatically</param>
        public void ShowStatusPopupPanel(string message, float selfDestroyTime = 1f)
        {
            if (UIRoot == null) return;

            ClearUIRoot();

            var panel = Instantiate(_statusPopupPanel, UIRoot.position, UIRoot.rotation) as GameObject;
            panel.transform.SetParent(UIRoot, false);

            var presenter = panel.GetComponent<StatusPopupPresenter>();
            presenter.UpdateMessage(message, selfDestroyTime);
        }

        void ClearUIRoot()
        {
            foreach (Transform t in UIRoot)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
