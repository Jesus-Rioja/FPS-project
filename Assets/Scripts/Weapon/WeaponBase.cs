using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayers = Physics.DefaultRaycastLayers;

    public bool canShootOnce;
    public bool canShootContinuously;

    public enum WeaponUseType
    {
        Shot,
        ContinuousShot,
        Undefined,
    };

    public virtual WeaponUseType GetUseType() { return WeaponUseType.Undefined; }

    public virtual void Swing()
    {

    }

    public virtual void Shot()
    {
        Debug.Log("Shot called in WeaponBase");
    }

    public virtual void StartShooting()
    {

    }

    public virtual void StopShooting()
    {

    }

    public virtual void EnableAnim()
    {

    }

    public virtual void DisableAnim()
    {

    }
}
