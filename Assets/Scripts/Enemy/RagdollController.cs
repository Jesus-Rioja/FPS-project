using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] float force = 30f;
    [SerializeField] float explosionForce = 80f;
    [SerializeField] float explosionUpForce = 40f;

    Collider[] colliders;
    Rigidbody[] rigidbodies;

    TargetWithLife targetWithLife;

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        targetWithLife = GetComponentInParent<TargetWithLife>();
    }

    private void OnEnable()
    {
        foreach(Collider collider in colliders) { collider.enabled = false; }
        foreach(Rigidbody rigidbody in rigidbodies) { rigidbody.isKinematic = true; }
        targetWithLife.onDeath.AddListener(NotifyDeath);
    }

    private void OnDisable()
    {
        targetWithLife.onDeath.RemoveListener(NotifyDeath);
    }


    void NotifyDeath(TargetWithLife target, TargetWithLife.DeathInfo deathInfo)
    {
        foreach (Collider collider in colliders) { collider.enabled = true; }
        foreach (Rigidbody rigidbody in rigidbodies) 
        {
            rigidbody.isKinematic = false;

            if (deathInfo.type == TargetWithLife.DamageType.Explosion){ rigidbody.AddExplosionForce(explosionForce, deathInfo.explosionPosition, deathInfo.explosionRadius, explosionUpForce); }
            else{ rigidbody.AddForce(deathInfo.direction * force, ForceMode.Impulse); }

        }
    }
}
