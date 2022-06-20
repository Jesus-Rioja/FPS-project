using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    WeaponMelee weaponMelee;
    void Awake()
    {
        weaponMelee = GetComponentInParent<WeaponMelee>();
    }

    void startSwing()
    {
        weaponMelee.Swing();
    }
}
