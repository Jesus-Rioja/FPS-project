using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TargetBase : MonoBehaviour
{
    [SerializeField] string[] tagsOfDamagingTargetEffects;
    public abstract void NotifyShot(float damage = 1f);
    public abstract void NotifySwing(float damage = 1f);
    public abstract void NotifyExplosion(float damage = 1f);
    public abstract void NotifyParticle(float damage = 1f);

    private void OnParticleCollision(GameObject other)
    {
        WeaponParticles weapon = other.GetComponentInParent<WeaponParticles>();

        // DIFERENCIAR POR TAG LAS PARTICULAS DA�INAS
        if (weapon)
            if (tagsOfDamagingTargetEffects.Contains("Fire")) { NotifyParticle(weapon.particleDamage); }
    }

}
