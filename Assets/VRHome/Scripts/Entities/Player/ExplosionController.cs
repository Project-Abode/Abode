using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public void SetColor(Color color)
    {
        if (_particleSystem != null) {
            _particleSystem.startColor = color;
            _particleSystem.Play();
        }
    }
}
