using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    [SerializeField] float forwardRange = 1f;
    [SerializeField] float horizontalRange = 1f;
    [SerializeField] float verticalRange = 1f;

    public override void Swing()
    {
        Vector3 halfExtents = new Vector3(horizontalRange / 2f, verticalRange / 2f, forwardRange / 2f);

        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, targetLayers);

        foreach (Collider c in colliders)
        {
                TargetBase targetBase = c.GetComponent<TargetBase>();
                targetBase?.NotifySwing();
        }
    }
}
