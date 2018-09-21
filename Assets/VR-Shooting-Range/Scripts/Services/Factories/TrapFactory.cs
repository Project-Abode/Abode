using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExitGames.SportShooting
{
    public class TrapFactory : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _trapObjects;

        [SerializeField]
        float _trapSpawnInterval;
        public float TrapSpawnInterval
        {
            get
            {
                return _trapSpawnInterval;
            }
        }

        [SerializeField]
        float _minTrapForce;

        [SerializeField]
        float _maxTrapFoce;
        public float RandomTrapForce
        {
            get
            {
                return Random.Range(_minTrapForce, _maxTrapFoce);
            }
        }

        [SerializeField]
        float _minXAngle;

        [SerializeField]
        float _maxXAngle;

        [SerializeField]
        float _minYAngle;

        [SerializeField]
        float _maxYAngle;

        public float RandomTrapXAngleDiffrence
        {
            get
            {
                return Random.Range(_minXAngle, _maxXAngle);
            }
        }
        public float RandomTrapYAngleDiffrence
        {
            get
            {
                return Random.Range(_minYAngle, _maxYAngle);
            }
        }

        [SerializeField]
        Transform _trapSpawnPointsRoot;
        
        List<Transform> _trapSpawnPoints;
        public Transform RandomTrapSpawnPoint
        {
            get
            {
                int trapIndex = Random.Range(0, _trapSpawnPoints.Count);                
                return _trapSpawnPoints[trapIndex];
            }
        }

        void Start()
        {
            _trapSpawnPoints = new List<Transform>();
            foreach (Transform childTransform in _trapSpawnPointsRoot)
            {
                _trapSpawnPoints.Add(childTransform);
            }
        }

        public void Build()
        {
            Transform spawnPoint = RandomTrapSpawnPoint;

            System.Random random = new System.Random();
            int trapIndex = random.Next(_trapObjects.Count);
            string prefabName = _trapObjects[trapIndex].name;

            Quaternion randAng = Quaternion.Euler(RandomTrapXAngleDiffrence, RandomTrapYAngleDiffrence, 0 );
            GameObject go = PhotonNetwork.Instantiate(prefabName, spawnPoint.position, randAng, group:0) as GameObject;
            go.GetComponent<Rigidbody>().AddForce(go.transform.forward * RandomTrapForce, ForceMode.Impulse);
        }
    }
}
 