using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetWithLife : TargetBase
{
    public enum DamageType
    {
        Shot,
        Swing,
        Explosion,
        Particle,
    }
    public struct DeathInfo
    {
        public DamageType type;

        public Vector3 direction;

        public Vector3 explosionPosition;
        public float explosionRadius;
    }

    [SerializeField] float lifeLostPerShot = 0.3f;
    [SerializeField] float lifeLostPerSwing = 0.4f;
    [SerializeField] float lifeLostPerExplosion = 20f;
    [SerializeField] float lifeLostPerParticle = 0.2f;
    [SerializeField] public UnityEvent<TargetWithLife, DeathInfo> onDeath;
    [SerializeField] public UnityEvent<TargetWithLife, float> onLifeLost;

    DeathInfo deathInfo = new DeathInfo();

    public float Life = 1f;

    public override void NotifyShot()
    {
        LoseLife(DamageType.Shot, lifeLostPerShot);
    }

    public override void NotifySwing()
    {
        LoseLife(DamageType.Swing, lifeLostPerSwing);
    }

    public override void NotifyExplosion()
    {
        LoseLife(DamageType.Explosion, lifeLostPerExplosion);
    }

    public override void NotifyParticle()
    {
        LoseLife(DamageType.Particle, lifeLostPerParticle);
    }

    protected virtual void LoseLife(DamageType damageType, float howMuch)
    {
        deathInfo.type = damageType;
        switch (deathInfo.type)
        {
            case DamageType.Shot:
                deathInfo.direction = transform.position - PlayerMovement.instance.transform.position;
                break;

            case DamageType.Swing:
                deathInfo.direction = transform.position - PlayerMovement.instance.transform.position;
                break;

            case DamageType.Explosion:
                deathInfo.explosionPosition = Explosion.lastExplosionPosition;
                deathInfo.explosionRadius = Explosion.lastExplosionRadius;
                break;

            case DamageType.Particle:
                deathInfo.direction = Vector3.zero;
                break;

            default:
                deathInfo.direction = transform.position - PlayerMovement.instance.transform.position;
                break;
        }

        Life -= howMuch;
        onLifeLost.Invoke(this, Life);
        CheckStillAlive();
    }

    protected virtual void CheckStillAlive()
    {
        if (Life <= 0)
        {
            onDeath.Invoke(this, deathInfo);
        }
    }
}
