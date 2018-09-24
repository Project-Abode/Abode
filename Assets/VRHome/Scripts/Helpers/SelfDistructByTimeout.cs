using UnityEngine;
using System.Collections;


namespace ExitGames.SportShooting
{
    public class SelfDistructByTimeout : MonoBehaviour
    {

        [SerializeField]
        float _disctructionTime;

        void Awake()
        {
            Destroy(gameObject, _disctructionTime);
        }        
    }
}
