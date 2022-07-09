using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentWeaponAmmoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text ammoMagazineText;
    [SerializeField] TMP_Text totalAmmoText;

    [SerializeField]WeaponBase currentWeapon;

    float auxTimer = 0;

    void Update()
    {

        if(auxTimer >= 0.5)
            { currentWeapon = PlayerMovement.instance.GetComponent<PlayerShooting>().currentWeapon; }
        else 
            { auxTimer += Time.deltaTime; }

        if(currentWeapon != null)
        {
            ammoMagazineText.text = currentWeapon.GetAmmoInCurrentMagazine() + "/" + currentWeapon.GetMagazineCapacity();
            if (!currentWeapon.consumesAmmo)
            { totalAmmoText.text = "Inf"; }
            else
            { totalAmmoText.text = "" + currentWeapon.GetCurrentAmmo(); }
        }

    }
}
