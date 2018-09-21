using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    /// <summary>
    /// Component's container for Player
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Transform _uiRoot;
        public Transform UIRoot
        {
            get
            {
                return _uiRoot;
            }
        }

        [SerializeField]
        private Transform _sideUIRoot;
        public Transform SideUIRoot
        {
            get
            {
                return _sideUIRoot;
            }
        }

        [SerializeField]
        GameObject _cameraRig;
        public GameObject CameraRig
        {
            get
            {
                return _cameraRig;
            }
        }

        [SerializeField]
        private GameObject _rifle;
        public GameObject Rifle
        {
            get { return _rifle; }      
        }

        [SerializeField]
        private GameObject _laserPointer;
        public GameObject LaserPointer
        {
            get { return _laserPointer; }   
        }
    }
}
