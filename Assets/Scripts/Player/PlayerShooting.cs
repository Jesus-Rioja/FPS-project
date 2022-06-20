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
    //[SerializeField] WeaponBase meleeWeapon;
    //[SerializeField] GameObject MeleeWeaponVisuals;
    [SerializeField] float MeleeAttackCooldown = 5f;
    float MeleeAttackTimer = 0f;
    [SerializeField] int MeleeAttackCharges = 5;
    public int CurrentMeleeAttackCharges = 5;

    Animator anim;
    bool ShootAllowed = true;
    bool MeleeAttackAllowed = true;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        /*MeleeWeaponVisuals.SetActive(false);*/
        CurrentMeleeAttackCharges = MeleeAttackCharges;
        MeleeAttackTimer = MeleeAttackCooldown;
    }

    void Update()
    {
        if (currentWeapon.canShootOnce)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(ShootAllowed)
                {
                    currentWeapon.Shot();
                    currentWeapon.EnableAnim();
                }
            }
        }
        else if(currentWeapon.canShootContinuously)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if(ShootAllowed)
                {
                    currentWeapon.StartShooting();
                    currentWeapon.EnableAnim();
                }
            }
            else
            {
                currentWeapon.StopShooting();
            }
        } 

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(MeleeAttackAllowed && CurrentMeleeAttackCharges > 0)
            {
                MeleeAttackAllowed = false;
                ShootAllowed = false;
                CurrentMeleeAttackCharges--;
                //MeleeWeaponVisuals.SetActive(true);
                //WeaponHandler.SetActive(false);
                //anim.SetTrigger("MeleeAttack");
                Invoke("MeleeAttack", 1f);
                Invoke("ActivateWeaponHandler", 2f);
            }
        }
        else
        {
            if (CurrentMeleeAttackCharges < MeleeAttackCharges)
            {
                MeleeAttackTimer -= Time.deltaTime;
                if (MeleeAttackTimer <= 0)
                {
                    CurrentMeleeAttackCharges = MeleeAttackCharges;
                    MeleeAttackTimer = MeleeAttackCooldown;
                }
            }
        }
    }

    public void ChangeCurrentWeapon(WeaponBase newWeapon)
    {
        currentWeapon?.StopShooting();
        currentWeapon = newWeapon;
    }

    void MeleeAttack()
    {
        MeleeAttackTimer = MeleeAttackCooldown;
        currentWeapon.Swing();
    }

    void ActivateWeaponHandler()
    {
        ShootAllowed = true;
        MeleeAttackAllowed = true;
        //MeleeWeaponVisuals.SetActive(false);
        //WeaponHandler.SetActive(true);
    }
}
