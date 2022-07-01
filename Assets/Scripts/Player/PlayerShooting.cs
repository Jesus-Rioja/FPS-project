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

    Animator anim;
    bool ShootAllowed = true;
    bool Swinging = false;
    bool MeleeAttackAllowed = true;
    GameObject currentWeaponVisuals;
    [SerializeField] CrosshairControl crosshairControl;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        CurrentMeleeAttackCharges = MeleeAttackCharges;
        MeleeAttackTimer = MeleeAttackCooldown;
    }

    void Update()
    {
        if (currentWeapon.canShootOnce)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(ShootAllowed && !Swinging)
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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if(ShootAllowed && !Swinging)
                {
                    crosshairControl.TriggerAnim();
                    currentWeapon.StartShooting();
                    currentWeapon.EnableAnim();
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
            if(MeleeAttackAllowed && CurrentMeleeAttackCharges > 0)
            {
                MeleeAttackAllowed = false;
                Swinging = true;
                CurrentMeleeAttackCharges--;
                currentWeaponVisuals = currentWeapon.gameObject.GetComponentInChildren<Visuals>().gameObject;
                currentWeaponVisuals.SetActive(false);
                anim.SetTrigger("MeleeAttack");
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
        meleeWeapon.Swing();
    }

    void ActivateWeaponHandler()
    {
        Swinging = false;

        currentWeaponVisuals.SetActive(true);
        MeleeAttackAllowed = true;
        //WeaponHandler.SetActive(true);
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
