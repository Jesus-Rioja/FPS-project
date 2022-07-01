using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponRaycast : WeaponMelee
{
    [Header("Weapon info")]
    //[SerializeField] float damage = 10f;
    [SerializeField] float range = 200f;
    [SerializeField] float shotCadence = 5f;
    [SerializeField] float projectilesPerShot = 1f;
    [SerializeField] protected Transform shootPoint;

    bool isShootingContinuously;
    float timeForNextShot = 0f;
    protected RaycastHit hit;


    AudioSource audioSource;

    [Header("Weapon projectile direction")]
    [SerializeField] float scatterAngle = 0f;

    [Header("Visuals & Audio")]
    [SerializeField] protected Transform flashPoint;
    [SerializeField] protected GameObject flashLight;
    [SerializeField] protected GameObject bloodSplatter;

    NoiseMaker noiseMaker;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        noiseMaker = GetComponentInChildren<NoiseMaker>();
    }

    void Update ()
    {
        timeForNextShot -= Time.deltaTime;
        timeForNextShot = timeForNextShot > 0f ? timeForNextShot : 0f;

        if (isShootingContinuously)
        {
            InternalShot();
        }
    }

    public override WeaponUseType GetUseType()
    {
        return WeaponUseType.Shot;
    }

    public override void Shot()
    {
        if(canShootOnce) { InternalShot(); }
    }

    protected void InternalShot()
    {        
        if (!isReloading && timeForNextShot <= 0f)
        {
            timeForNextShot += 1f / shotCadence;

            Instantiate(flashLight, flashPoint.position, Quaternion.identity);
            audioSource?.Play();
            noiseMaker?.MakeNoise();

            if (UseAmmo() == UseAmmoResult.ShotMade)  
            {
                for (int i = 0; i < projectilesPerShot; i++)
                {

                    float horizontalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
                    Quaternion horizontalScatter = Quaternion.AngleAxis(horizontalScatterAngle, transform.up);

                    float verticalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
                    Quaternion verticalScatter = Quaternion.AngleAxis(verticalScatterAngle, transform.up);

                    Vector3 shotForward = verticalScatter * (horizontalScatter * shootPoint.forward);

                    Ray ray = new Ray(shootPoint.position, shotForward);

                    if (Physics.Raycast(ray, out hit, range, targetLayers, QueryTriggerInteraction.Ignore))
                    {
                        Debug.DrawLine(shootPoint.position, hit.point, Color.cyan, 10f);
                        Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);

                        TargetBase targetBase = hit.collider.GetComponent<TargetBase>();

                        if (hit.collider.tag == "Enemy")
                            Instantiate(bloodSplatter, hit.point, hit.transform.rotation);

                        targetBase?.NotifyShot(CalcDamage(targetBase.transform.position));
                    }

                }
            }
        }
    }

    public override void StartShooting()
    {
        isShootingContinuously = canShootContinuously;
    }

    public override void StopShooting()
    {
        isShootingContinuously = false;
    }

}
