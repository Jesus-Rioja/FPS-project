using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayers = Physics.DefaultRaycastLayers;

    public bool canShootOnce;
    public bool canShootContinuously;

    [SerializeField] float minRange = 1f;
    [SerializeField] float maxRange = 25f;

    [SerializeField] float minDamage = 1f;
    [SerializeField] float maxDamage = 0.25f;

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

    protected float CalcDamage(Vector3 hitPosition)
    {
        return CalcDamage(Vector3.Distance(transform.position, hitPosition));
    }

    protected float CalcDamage(float distance)
    {
        if(distance < minDamage) { return maxDamage; }
        if(distance > maxDamage) { return 0f; }
        return Mathf.Lerp(maxDamage, minDamage, (distance - minRange) / (maxDamage - minRange));
    }

}
