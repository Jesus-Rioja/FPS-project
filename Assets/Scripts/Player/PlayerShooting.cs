using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerShooting : MonoBehaviour
{
    WeaponBase currentWeapon;
    [Header("Range Weapons info")]
    [SerializeField] GameObject WeaponHandler;

    [Header("Melee Weapon info")]
    [SerializeField] WeaponBase meleeWeapon;
    [SerializeField] float MeleeAttackCooldown = 5f;
    float MeleeAttackTimer = 0f;
    [SerializeField] int MeleeAttackCharges = 5;
    public int CurrentMeleeAttackCharges = 5;

    [SerializeField] CrosshairControl crosshairControl;



    private void Awake()
    {
        CurrentMeleeAttackCharges = MeleeAttackCharges;
        MeleeAttackTimer = MeleeAttackCooldown;
    }

    void Update()
    {
        if (currentWeapon.canShootOnce)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && currentWeapon.isUsable)
            {
                if (currentWeapon.NeedsReload())
                { currentWeapon.Reload(); }
                else if (currentWeapon.HasAmmo())
                {
                    currentWeapon.Shot();
                    currentWeapon.EnableAnim();
                }
                else
                    Debug.Log("quiero disparar");
            }
        }
        else if(currentWeapon.canShootContinuously)
        {
            if (Input.GetKey(KeyCode.Mouse0) && currentWeapon.isUsable)
            {
                if (currentWeapon.NeedsReload())
                { currentWeapon.Reload(); }
                else if (currentWeapon.HasAmmo())
                {
                    crosshairControl.TriggerAnim();
                    currentWeapon.StartShooting();
                    currentWeapon.EnableAnim();
                }
                else
                {
                    currentWeapon.StopShooting();
                }
            }
            else
            {
                currentWeapon.StopShooting();
            }
        } 

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.Reload();
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
             currentWeapon.Swing();
        }

    }

    public void ChangeCurrentWeapon(WeaponBase newWeapon)
    {
        currentWeapon?.StopShooting();
        currentWeapon = newWeapon;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            currentWeapon.AddAmmo();
            Destroy(other.gameObject);
        }
    }
}
