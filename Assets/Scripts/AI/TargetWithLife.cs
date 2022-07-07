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

    [SerializeField] public UnityEvent<TargetWithLife, DeathInfo> onDeath;
    [SerializeField] public UnityEvent<TargetWithLife, float> onLifeLost;

    bool Invulnerable = false;

    DeathInfo deathInfo = new DeathInfo();

    public float Life = 1f;
    [SerializeField] float maxLife = 1f;
    [SerializeField] float medikitLifeRecovey = 10f;

    [SerializeField] float timeToRegenerateLife = 10f;
    float regenerateLifeTimer;
    float lifeTickTimer;
    float timePerTick = 2f;

    private void Start()
    {
        lifeTickTimer = timePerTick;
        regenerateLifeTimer = timeToRegenerateLife;
        Life = maxLife;
    }

    private void Update()
    {
        regenerateLifeTimer -= Time.deltaTime;
        if(regenerateLifeTimer <= 0)
        {
            lifeTickTimer -= Time.deltaTime;
            if(lifeTickTimer <= 0)
            {
                lifeTickTimer = timePerTick;
                Life += 1;
                if(Life >= maxLife) { Life = maxLife; }
            }
        }
    }

    public override void NotifyShot(float damage)
    {
        LoseLife(DamageType.Shot, damage);
    }

    public override void NotifySwing(float damage)
    {
        if (!Invulnerable)
        {
            Invulnerable = true;
            LoseLife(DamageType.Swing, damage);
            Invoke("DisableInvulnerability", 0.1f);
        }
    }

    public override void NotifyExplosion(float damage)
    {
        LoseLife(DamageType.Explosion, damage);
    }

    public override void NotifyParticle(float damage)
    {
        LoseLife(DamageType.Particle, damage);
    }

    protected virtual void LoseLife(DamageType damageType, float howMuch)
    {
        regenerateLifeTimer = timeToRegenerateLife;
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

    void DisableInvulnerability()
    {
        Invulnerable = false;
    }

    public void regenerateLife()
    {
        Life = maxLife;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MediKit"))
        {
            //TODO: disallow recovery more
            //  life than original life max value
            Life += medikitLifeRecovey;
            if (Life >= maxLife) { Life = maxLife; }
            Destroy(other.gameObject);
        }
    }
}
