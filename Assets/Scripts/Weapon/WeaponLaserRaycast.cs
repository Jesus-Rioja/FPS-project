using System;
using System.Diagnostics;
using UnityEngine;

public class WeaponLaserRaycast : WeaponBase
{
    [Header("Weapon info")]
    //[SerializeField] float damage = 10f;
    [SerializeField] float range = 200f;
    [SerializeField] float fireRate = 3f;

    [SerializeField] LayerMask layerMaskAimingDetection;


    //public Camera fpscamera;
    //[SerializeField] ParticleSystem muzzleflash;
    //[SerializeField] GameObject impactEffect;

    [SerializeField] float nextTimeToFire = 0f;

    [SerializeField] Transform shootPoint;

    Ray MouseRay;

    LineRenderer lineRenderer;
    AudioSource audioSource;

    [SerializeField] float laserWidth = 0.1f;

    [Header("Debug")]
    [SerializeField] bool debugShot;

    //Vector3 desiredForward = Vector3.zero;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        lineRenderer.SetPositions(initLaserPositions);
        //lineRenderer.SetWidth(laserWidth, laserWidth);
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    // Update is called once per frame
    void Update ()
    {
        MouseRay = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shot();
            lineRenderer.enabled = true;
        }
        else { lineRenderer.enabled = false; }
    }

    private void OnValidate()
    {
        if (debugShot)
        {
            Shot();
            debugShot = false;
        }
    }

    public override WeaponUseType GetUseType()
    {
        return WeaponUseType.Shot;
    }

    public override void Shot()
    {
        //muzzleflash.Play();
        audioSource.Play();
        RaycastHit hit;
        Vector3 desiredForward = Vector3.zero;

        if (Physics.Raycast(MouseRay, out hit, Mathf.Infinity, layerMaskAimingDetection))
        {
            desiredForward = hit.point - transform.position;
            desiredForward.y = 0;

            Ray ray = new Ray(shootPoint.position, desiredForward);

            if (Physics.Raycast(ray, out hit, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                TargetBase targetBase = hit.collider.GetComponent<TargetBase>();

                targetBase?.NotifyShot();

                /*if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }*/

                lineRenderer.SetPosition(0, shootPoint.position);
                lineRenderer.SetPosition(1, hit.point);

                /*GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);*/
            }
        }

    }
}
