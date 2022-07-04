using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayers = Physics.DefaultRaycastLayers;
    [SerializeField] public float rechargeTime = 0f;

    public UnityEvent<WeaponBase, float> RechargeTimeEvent;

    public bool canShootOnce;
    public bool canShootContinuously;
    public bool ShotAllowed = true;

    [SerializeField] float minRange = 1f;
    [SerializeField] float maxRange = 25f;

    [SerializeField] float minDamage = 1f;
    [SerializeField] float maxDamage = 0.25f;

    public bool isUsable = true;


    public enum WeaponUseType
    {
        Swing,
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
        if(distance < minRange) { return maxDamage; }
        if(distance > maxRange) { return 0f; }

        float finalDamage = Mathf.Lerp(maxDamage, minDamage, (distance - minRange) / (maxDamage - minRange));

        return finalDamage;
    }

    public float GetMaxRange() { return maxRange; }
    public float GetMinRange() { return minRange; }

    [Header("Ammo and Magazines, reload")]
    [SerializeField] int maxAmmo = 100;
    [SerializeField] int currentAmmo = 24;
    [SerializeField] int ammoInCurrentMagazine = 12; //AÑADIR EN START QUE NO PUEDA SER MAYOR A 12
    [SerializeField] int magazineCapacity = 12;
    [SerializeField] float reloadTime = 5f;
    [SerializeField] bool consumesAmmo = true;
    protected bool isReloading;

    protected enum UseAmmoResult
    {
        ShotMade,
        NeedsReload,
        NoAmmo,
    };

    protected UseAmmoResult UseAmmo()
    {
        if(currentAmmo == 0) return UseAmmoResult.NoAmmo;
        if(ammoInCurrentMagazine == 0) return UseAmmoResult.NeedsReload;

        if(consumesAmmo)
            currentAmmo--;

        ammoInCurrentMagazine--;
        return UseAmmoResult.ShotMade;
    }

    internal void AddAmmo()
    {
        currentAmmo += magazineCapacity;
    }

    public bool HasAmmo() { return currentAmmo > 0; }
    public bool NeedsReload() { return HasAmmo() && (ammoInCurrentMagazine == 0); }

    [SerializeField] protected Animator anim;

    public void Reload()
    {
        if(isUsable)
        {
            if(anim != null) { anim.SetTrigger("Reload"); }

            Debug.Log("Empiezo a recargar");
            isUsable = false;
            isReloading = true;
            Invoke(nameof(ReloadAfterSeconds), reloadTime); 
        }
    }

    void ReloadAfterSeconds()
    {
        Debug.Log("Acabo de recargar");
        isUsable = true;
        isReloading = false;
        ammoInCurrentMagazine = Mathf.Min(magazineCapacity, currentAmmo);
    }

}
