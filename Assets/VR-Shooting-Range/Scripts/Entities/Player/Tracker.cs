using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    /// <summary>
    /// Track and syncrhonize with Target object
    /// </summary>
    public class Tracker : MonoBehaviour
    {
        [SerializeField]
        Transform _target;

        [SerializeField]
        bool _trackPosition;
        [SerializeField]
        bool _trackRotation;

        void Update()
        {
            if(_target == null)
            {
                return;
            }

            if (_trackPosition)
            {
                transform.position = _target.position;
            }

            if (_trackRotation)
            {
                transform.rotation = _target.rotation;
            }
        }
    }
}
