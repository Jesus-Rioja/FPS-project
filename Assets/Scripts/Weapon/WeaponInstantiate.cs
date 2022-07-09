using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstantiate : WeaponMelee
{
    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform shootPoint;
    [SerializeField] float forceToApplyOnShot = 300f;
    [SerializeField] float shotCadence = 0.25f;

    [SerializeField] AudioSource ShootSound;
    [SerializeField] AudioSource ExplosionSound;

    float timeForNextShot = 0f;


    private void Update()
    {
        timeForNextShot -= Time.deltaTime;
        timeForNextShot = timeForNextShot > 0f ? timeForNextShot : 0f;
    }

    public override void Shot()
    {
        if (!isReloading && timeForNextShot <= 0)
        {
            timeForNextShot = 1f / shotCadence;
            if (UseAmmo() == UseAmmoResult.ShotMade)
            { 
                ShootSound.Play();
                CinemachineShake.Instance.ShakeCamera(.25f, .05f);

                GameObject proyectil = Instantiate(prefabProyectil, shootPoint.position, shootPoint.rotation);
                proyectil.GetComponent<Rigidbody>()?.AddForce(shootPoint.forward * forceToApplyOnShot);
                proyectil.GetComponent<OnCollisionDestroy>().ExplosionSound.AddListener(PlayExplosionSound);
            }

        }
    }

    public void PlayExplosionSound()
    {
        ExplosionSound.Play();
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

}
