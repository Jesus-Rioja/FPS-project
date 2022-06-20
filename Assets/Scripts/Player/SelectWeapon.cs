using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectWeapon : MonoBehaviour
{
    [SerializeField] int CurrentWeapon = 0;
    [SerializeField] UnityEvent<int> onWeaponChanged;
    PlayerShooting playerShooting;

    private void Awake()
    {
        playerShooting = GetComponentInParent<PlayerShooting>();
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
            if (CurrentWeapon >= transform.childCount - 1)
                CurrentWeapon = 0;
            else
                CurrentWeapon++;
        }
        if (Input.GetButtonDown("PreviousWeapon")) 
        {
            if (CurrentWeapon <= 0)
                CurrentWeapon = transform.childCount - 1;
            else
                CurrentWeapon--;
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
}
