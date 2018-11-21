using ExitGames.SportShooting;
using UnityEngine;

namespace Exitgames.SportShooting
{
    public class NonVrController : MonoBehaviour
    {
        [Range(0.1f, 10.0f)]
        public float MouseSensitivity = 2.0f;
        
        [SerializeField]
        private LayerMask hitLayer;

        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private GameObject canvas;

        private AudioSource shotSound;

        private float rotationX, rotationY;

        public void Awake()
        {
            shotSound = GetComponent<AudioSource>();

            //Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            if (!PhotonNetwork.player.IsLocal && !photonView.isMine)
            {
                return;
            }

            MenuInteraction();
            LookAround();

            if (PhotonNetwork.connected && PhotonNetwork.inRoom)
            {
                FireWeapon();
            }
        }

        public void OnGUI()
        {
            if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Press 'RETURN' to begin.");
                GUILayout.EndVertical();
            }

            if (!photonView.isMine)
            {
                return;
            }

            if (PhotonNetwork.connected && PhotonNetwork.inRoom)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Press 'ESC' to leave the game.");
                GUILayout.Space(100.0f);

                foreach (PhotonPlayer p in PhotonNetwork.playerList)
                {
                    string text = p.CustomProperties["name"] + ": " + p.CustomProperties["roundScore"];
                    GUILayout.Label(text);
                }

                GUILayout.EndVertical();
            }
        }

        private void LookAround()
        {
            rotationX += Input.GetAxis("Mouse X") * MouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * MouseSensitivity;

            rotationX = (rotationX < -360.0f) ? (rotationX + 360.0f) : ((rotationX > 360.0f) ? (rotationX - 360.0f) : rotationX);
            rotationY = (rotationY < -360.0f) ? (rotationY + 360.0f) : ((rotationY > 360.0f) ? (rotationY - 360.0f) : rotationY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

            transform.rotation = Quaternion.identity * xQuaternion * yQuaternion;
        }

        private void MenuInteraction()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
                {
//                    GameController.Instance.StartMultiplayerGame();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PhotonNetwork.connected && PhotonNetwork.inRoom)
                {
                    NetworkController.Instance.EndMultiplayerGame();
//                    GameController.Instance.InitMainMenu();

                    PhotonNetwork.player.CustomProperties.Clear();
                }
            }
        }

        private void FireWeapon()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 1000, hitLayer))
                {
                    var hitObject = hit.collider.GetComponent<Distructable>();

                    if (hitObject != null)
                    {
                        hitObject.MarkToDestroy();
                    }
                }

                photonView.RPC("PlayShotSound", PhotonTargets.All);
            }
        }

        [PunRPC]
        public void PlayShotSound()
        {
            shotSound.PlayOneShot(shotSound.clip);
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            canvas.SetActive(photonView.isMine);
        }
    }
}