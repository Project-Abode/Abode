using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepParticles : MonoBehaviour
{

    private ParticleSystem particleSys;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[0];

    private void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    public void SetParticles(ParticleSystem.Particle[] particles)
    {
        this.particles = particles;
    }

    private void Update()
    {
        particleSys.SetParticles(particles, particles.Length);
    }
}
