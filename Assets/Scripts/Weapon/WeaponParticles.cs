using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticles : WeaponBase
{
    ParticleSystem.EmissionModule[] emissionModule;
    [SerializeField] public float particleDamage { get; private set; }

    private void Awake()
    {
        ParticleSystem[] particleSystem = GetComponentsInChildren<ParticleSystem>();
        emissionModule = new ParticleSystem.EmissionModule[particleSystem.Length];

        for (int i = 0; i < particleSystem.Length; i++)
        {
            emissionModule[i] = particleSystem[i].emission;
            emissionModule[i].enabled = false;
        }
    }

    public override void StartShooting()
    {
        for (int i = 0; i < emissionModule.Length; i++)
        {
            emissionModule[i].enabled = true;
        }
    }

    public override void StopShooting()
    {
        for (int i = 0; i < emissionModule.Length; i++)
        {
            emissionModule[i].enabled = false;
        }
    }
}
