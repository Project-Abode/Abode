using UnityEngine;
using Photon;

namespace ExitGames.SportShooting
{
    public class Distructable : PunBehaviour
    {
        [SerializeField]
        GameObject _explosionPrefab;

        [SerializeField]
        GameObject _scorePrefab;

        [SerializeField]
        Color _color;

        [SerializeField]
        int _hitValue = 0;

        [SerializeField]
        bool _distructableByHit = true;

        private bool _isHitted = false;
        
        void OnCollisionEnter(Collision col)
        {
            photonView.RPC("DestroyByCollision", PhotonTargets.All);
        }

        public void MarkToDestroy()
        {
            // Send message to the Master client that we hit the target
            photonView.RPC("DestroyByHit", PhotonTargets.AllViaServer, PhotonNetwork.player.ID);
        }

        [PunRPC]
        void DestroyByCollision()
        {
            if (photonView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        [PunRPC]
        void DestroyByHit(int hittedPlayerID)
        {
            CalculateScore(hittedPlayerID);
            ExplodeByHit();
            
            if (_distructableByHit)
            {
                Instantiate(Resources.Load("Helper/TargetHit"), transform.position, transform.rotation);

                if (photonView.isMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }            
            }
            else
            {
                Instantiate(Resources.Load("Helper/BadTarget"), transform.position, transform.rotation);
            }
        }

        private void CalculateScore(int hittedPlayerID)
        {
            if (hittedPlayerID >= 0 && PhotonNetwork.isMasterClient && !_isHitted)
            {
                _isHitted = _distructableByHit;
                GameModel.Instance.CountScoreToPlayer(hittedPlayerID, _hitValue);
            }
        }

        private void ExplodeByHit()
        {
            if (_distructableByHit && _explosionPrefab != null)
            {
                var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity) as GameObject;
                var rigidbody = explosion.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.velocity = GetComponent<Rigidbody>().velocity;
                }

                ExplosionController explosionController = explosion.GetComponent<ExplosionController>();
                if (explosionController != null)
                {
                    explosionController.SetColor(_color);
                }
            }

            if (_scorePrefab != null)
            {
                var score = Instantiate(_scorePrefab, transform.position, Quaternion.identity) as GameObject;
                var text = score.GetComponent<ScoreTextField>();
                if (text != null)
                {
                    text.SetValue(_hitValue);
                }
            }
        }
    }
}
