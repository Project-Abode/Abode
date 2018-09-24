using UnityEngine;
using UnityEngine.VR;

namespace ExitGames.SportShooting
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField]
        bool useNonVrPlayerInEditor;

        [SerializeField]
        GameObject _ovrPlayerPrefab;

        [SerializeField]
        GameObject _googleVrPlayerPrefab;

        [SerializeField]
        GameObject _steamVrPlayerPrefab;

        [SerializeField]
        GameObject _nonVrPlayerPrefab;

        [SerializeField]
        Transform _playerSpawnPoints;

        private GameObject _playerPrefab;

        private void Awake()
        {
            #if UNITY_EDITOR
            if (useNonVrPlayerInEditor)
            {
                _playerPrefab = _nonVrPlayerPrefab;
                //_playerPrefab = _steamVrPlayerPrefab;
                return;
            }
            #else
            useNonVrPlayerInEditor = false;
            #endif

            #if UNITY_STANDALONE
            if (UnityEngine.XR.XRSettings.loadedDeviceName == "Oculus")
            {             
                _playerPrefab = _ovrPlayerPrefab;
            }
            else
            {            
                _playerPrefab = _steamVrPlayerPrefab;
            }
            #elif UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
            _playerPrefab = _googleVrPlayerPrefab;           
            #endif
        }

        public Transform PlayerSpawnPoints
        {
            get
            {
                return _playerSpawnPoints;
            }
        }

        public void Build()
        {
            if (GameModel.Instance.ActiveGameState is InitializingGameState)
            {
                BuildPlayerForGame();
            }
            else if (GameModel.Instance.ActiveGameState is MainMenuGameState)
            {
                BuildPlayerForMenu();
            }
        }

        public void BuildPlayerForGame()
        {
            if (GameModel.Instance.CurrentPlayer != null)
            {
                GameObject.DestroyImmediate(GameModel.Instance.CurrentPlayer.gameObject);
            }

            int positionIndex = (int)PhotonNetwork.player.CustomProperties["position"];
            Vector3 spawnPoint = PlayerSpawnPoints.GetChild(positionIndex).position;
            
            if (useNonVrPlayerInEditor)
            {
                spawnPoint.y += 2;
            }

            GameObject go = PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint, Quaternion.identity, 0) as GameObject;
            GameModel.Instance.CurrentPlayer = go.GetComponent<Player>();

            //Initialize UI
            if (!useNonVrPlayerInEditor)
            {
                GameModel.Instance.CurrentPlayer.CameraRig.SetActive(true);
                GameModel.Instance.CurrentPlayer.UIRoot.gameObject.SetActive(true);
                GameModel.Instance.CurrentPlayer.SideUIRoot.gameObject.SetActive(true);
                GameView.Instance.UIRoot = GameModel.Instance.CurrentPlayer.UIRoot;
                GameView.Instance.SideUIRoot = GameModel.Instance.CurrentPlayer.SideUIRoot;
            }
        }

        public void BuildPlayerForMenu()
        {
            if (GameModel.Instance.CurrentPlayer != null)
            {
                PhotonNetwork.Destroy(GameModel.Instance.CurrentPlayer.gameObject);
            }

            Vector3 spawnPoint = PlayerSpawnPoints.GetChild(0).position;
            
            if (useNonVrPlayerInEditor)
            {
                spawnPoint.y += 2;
            }

            GameObject go = GameObject.Instantiate(_playerPrefab, spawnPoint, Quaternion.identity) as GameObject;
            GameModel.Instance.CurrentPlayer = go.GetComponent<Player>();

            //Initializing UI
            if (!useNonVrPlayerInEditor)
            {
                GameModel.Instance.CurrentPlayer.CameraRig.SetActive(true);
                GameModel.Instance.CurrentPlayer.Rifle.SetActive(false);
                GameModel.Instance.CurrentPlayer.LaserPointer.SetActive(true);
                GameModel.Instance.CurrentPlayer.UIRoot.gameObject.SetActive(true);
                GameView.Instance.UIRoot = GameModel.Instance.CurrentPlayer.UIRoot;
                GameView.Instance.SideUIRoot = GameModel.Instance.CurrentPlayer.SideUIRoot;
            }
        }

        public static Color GetColor(int position)
        {
            switch (position)
            {
                case 0: return Color.red;
                case 1: return Color.blue;
                case 2: return Color.yellow;
                case 3: return Color.green;
                case 4: return Color.black;
                default: return Color.grey;
            }
        }
    }
}
