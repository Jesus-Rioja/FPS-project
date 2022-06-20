using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponRaycast : WeaponBase
{
    [Header("Weapon info")]
    //[SerializeField] float damage = 10f;
    [SerializeField] float range = 200f;
    [SerializeField] float fireRate = 5f;
    [SerializeField] protected Transform shootPoint;
    bool isShootingContinuously;
    float timeForNextShot = 0f;
    protected RaycastHit hit;

    //VISUALS AND AUDIO
    protected LineRenderer lineRenderer;
    [SerializeField] float laserWidth = 0.1f;

    AudioSource audioSource;

    [Header("Weapon projectile direction")]
    [SerializeField] float scatterAngle = 0f;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new Vector3[2] { shootPoint.position, shootPoint.position });

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    void Update ()
    {
        if (isShootingContinuously)
        {
            timeForNextShot -= Time.deltaTime;
            if(timeForNextShot <= 0f)
            {
                InternalShot();
                timeForNextShot += 1f / fireRate;
            }
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
        audioSource?.Play();
        lineRenderer.SetPosition(0, shootPoint.position);

        float horizontalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
        Quaternion horizontalScatter = Quaternion.AngleAxis(horizontalScatterAngle, transform.up);

        float verticalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
        Quaternion verticalScatter = Quaternion.AngleAxis(verticalScatterAngle, transform.up);

        Vector3 shotForward = verticalScatter * (horizontalScatter * shootPoint.forward);

        Ray ray = new Ray(shootPoint.position, shotForward);

        if (Physics.Raycast(ray, out hit, range, targetLayers, QueryTriggerInteraction.Ignore))
        {
            lineRenderer.SetPosition(1, hit.point);

            Debug.DrawLine(shootPoint.position, hit.point, Color.cyan, 10f);
            Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);

            TargetBase targetBase = hit.collider.GetComponent<TargetBase>();

            targetBase?.NotifyShot();
        }
        StartCoroutine(ShootLaser());
    }

    public override void StartShooting()
    {
        isShootingContinuously = canShootContinuously;
    }

    public override void StopShooting()
    {
        isShootingContinuously = false;
    }

    IEnumerator ShootLaser()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.05f);
        lineRenderer.enabled = false;
    }
}
