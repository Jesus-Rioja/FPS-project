using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMeleeCharges : MonoBehaviour
{
    TMP_Text MeleeChargesText;
    Image image;
    PlayerShooting playerShooting;
    int CurrentMeleeWeaponCharges;

    private void Awake()
    {
        MeleeChargesText = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        playerShooting = PlayerMovement.instance.transform.GetComponent<PlayerShooting>();
        CurrentMeleeWeaponCharges = playerShooting.CurrentMeleeAttackCharges;
        MeleeChargesText.text = CurrentMeleeWeaponCharges.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentMeleeWeaponCharges != playerShooting.CurrentMeleeAttackCharges)
        {
            CurrentMeleeWeaponCharges = playerShooting.CurrentMeleeAttackCharges;
            MeleeChargesText.text = CurrentMeleeWeaponCharges.ToString();
            if(CurrentMeleeWeaponCharges <= 0) 
            {
                var tempColor = image.color;
                tempColor.a = 0.25f;
                image.color = tempColor;
            }
            else
            {
                var tempColor = image.color;
                tempColor.a = 1f;
                image.color = tempColor;
            }

        }
    }
}
