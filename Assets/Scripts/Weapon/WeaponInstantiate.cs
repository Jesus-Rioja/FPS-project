using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstantiate : WeaponBase
{
    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform shootPoint;
    [SerializeField] float forceToApplyOnShot = 300f;

    //float rechargeTime = 1f;
    //bool ShotAllowed = true;

    private void OnEnable()
    {
        StartRecharge();
    }

    public override void Shot()
    {
        if (ShotAllowed)
        {
            GameObject proyectil = Instantiate(prefabProyectil, shootPoint.position, shootPoint.rotation);
            proyectil.GetComponent<Rigidbody>()?.AddForce(shootPoint.forward * forceToApplyOnShot);
            StartCoroutine(StartRecharge());
        }
    }

    public override void Swing()
    {
        base.Swing();
    }

    public override void StartShooting()
    {
        base.StartShooting();
    }

    public override void StopShooting()
    {
        base.StopShooting();
    }

    private IEnumerator StartRecharge()
    {
        ShotAllowed = false;
        yield return new WaitForSeconds(rechargeTime);
        ShotAllowed = true;
    }
}
