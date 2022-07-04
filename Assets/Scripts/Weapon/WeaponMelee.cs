using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    [SerializeField] Transform meleePoint;
    [SerializeField] float forwardRange = 1f;
    [SerializeField] float horizontalRange = 1f;
    [SerializeField] float verticalRange = 1f;

    [SerializeField] float meleeDamage = 0.25f;
    [SerializeField] float meleeCadence = 0.25f;

    private void Start()
    {
        meleePoint = meleePoint ? meleePoint : transform; //Si meleePoint existe, nos quedamos con el. si no existe, cogemos nuestro transform
    }

    public override void Swing() //HA DICHO ENRIQUE QUE ESTA MAL ESTA FUNCION
    {
        if (isUsable)
        {
            anim.SetTrigger("MeleeAttack");
            isUsable = false;
            Invoke(nameof(SwingEnd), (1f / meleeCadence));

            Vector3 halfExtents = new Vector3(horizontalRange / 2f, verticalRange / 2f, forwardRange / 2f);

            Collider[] colliders = Physics.OverlapBox(meleePoint.position, halfExtents, meleePoint.rotation, targetLayers);

            foreach (Collider c in colliders)
            {
                TargetBase targetBase = c.GetComponent<TargetBase>();
                targetBase?.NotifySwing(meleeDamage);
            }
        }
    }

    void SwingEnd()
    {
        Debug.Log("termino melee");
        isUsable = true;
    }
}
