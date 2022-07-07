using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectWeapon : MonoBehaviour
{
    [SerializeField] int CurrentWeapon = 0;
    [SerializeField] UnityEvent<int> onWeaponChanged;
    PlayerShooting playerShooting;

    int[] weaponAcquired;


    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        playerShooting = GetComponentInParent<PlayerShooting>();
        weaponAcquired = new int[4];
        WeaponsUnlock("Machine gun");
    }

    private void Start()
    {
        ChangeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = CurrentWeapon;

        if (Input.GetButtonDown("NextWeapon")) 
        {
            do {
                if (CurrentWeapon >= transform.childCount - 1)
                    CurrentWeapon = 0;
                else
                    CurrentWeapon++;

            } while (weaponAcquired[CurrentWeapon] == 0);

        }
        if (Input.GetButtonDown("PreviousWeapon")) 
        {
            do {
                if (CurrentWeapon <= 0)
                    CurrentWeapon = transform.childCount - 1;
                else
                    CurrentWeapon--;

            } while (weaponAcquired[CurrentWeapon] == 0);

        }

        if (previousWeapon != CurrentWeapon)
            ChangeWeapon();
    }

    void ChangeWeapon()
    {
        int i = 0;

        foreach(Transform weapon in transform)
        {
            if (i == CurrentWeapon)
            {
                weapon.gameObject.SetActive(true);
                playerShooting.ChangeCurrentWeapon(weapon.GetComponent<WeaponBase>());
                onWeaponChanged.Invoke(CurrentWeapon);
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    public IEnumerator StartRechargeTime(WeaponBase weapon, float time)
    {
        weapon.ShotAllowed = false;
        yield return new WaitForSeconds(time);
        weapon.ShotAllowed = true;
    }

    public void StartRechargeTimeEventTrigger(WeaponBase weapon, float time)
    {
        StartCoroutine(StartRechargeTime(weapon, time));
    }

    public void WeaponsUnlock(string weaponName)
    {
        PlayerPrefs.SetInt(weaponName, 1);

        weaponAcquired[0] = PlayerPrefs.GetInt("Machine gun", 0);
        weaponAcquired[1] = PlayerPrefs.GetInt("Shotgun", 0);
        weaponAcquired[2] = PlayerPrefs.GetInt("Grenade launcher", 0);
        weaponAcquired[3] = PlayerPrefs.GetInt("Flamethrower", 0);
    }

}
