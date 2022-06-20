using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TargetBase : MonoBehaviour
{
    [SerializeField] string[] tagsOfDamagingTargetEffects;
    public abstract void NotifyShot();
    public abstract void NotifySwing();
    public abstract void NotifyExplosion();
    public abstract void NotifyParticle();

    private void OnParticleCollision(GameObject other)
    {
        // DIFERENCIAR POR TAG LAS PARTICULAS DAÑINAS
        if (tagsOfDamagingTargetEffects.Contains("Fire")) { NotifyParticle(); }
    }

}
