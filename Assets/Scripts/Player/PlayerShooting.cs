using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerShooting : MonoBehaviour
{
    public WeaponBase currentWeapon;
    [Header("Range Weapons info")]
    [SerializeField] GameObject WeaponHandler;

    [SerializeField] CrosshairControl crosshairControl;

    float runAvaiableTime = 0f;

    void Update()
    {
        if (Time.timeScale > 0)
        {

            if (currentWeapon.canShootOnce)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && currentWeapon.isUsable)
                {
                    if (currentWeapon.NeedsReload())
                    { currentWeapon.Reload(); }
                    else if (currentWeapon.HasAmmo())
                    {
                        runAvaiableTime = 0.5f;
                        PlayerMovement.instance.canRun = false;
                        currentWeapon.Shot();
                        currentWeapon.EnableAnim();
                    }
                    else
                        Debug.Log("quiero disparar");
                }
                else
                {
                    runAvaiableTime -= Time.deltaTime;
                    if (runAvaiableTime <= 0)
                    { PlayerMovement.instance.canRun = true; }
                }
            }
            else if (currentWeapon.canShootContinuously)
            {
                if (Input.GetKey(KeyCode.Mouse0) && currentWeapon.isUsable)
                {
                    if (currentWeapon.NeedsReload())
                    { currentWeapon.Reload(); }
                    else if (currentWeapon.HasAmmo())
                    {
                        runAvaiableTime = 0.5f;
                        PlayerMovement.instance.canRun = false;
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
                    runAvaiableTime -= Time.deltaTime;
                    if (runAvaiableTime <= 0)
                    { PlayerMovement.instance.canRun = true; }
                    currentWeapon.StopShooting();
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && currentWeapon.HasAmmo())
            {
                currentWeapon.Reload();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                currentWeapon.Swing();
            }
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
