using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This component is obsolete. Please use WeaponRaycast with projectilesPerShot > 1")]
public class WeaponRaycastMultiShot : WeaponRaycast
{
    [SerializeField] int numShots;

    //bool ShotAllowed = true;
    //float rechargeTime = 1f;
    int index = 0;

    private void OnEnable()
    {
        StartCoroutine(StartRecharge());
        lineRenderer.positionCount = (numShots*2) + 1;
    }

    private void OnDisable()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[2] { shootPoint.position, shootPoint.position });
    }

    public override void Shot()
    {
        Debug.Log("Disparo ecopeta");
        if(ShotAllowed)
        {
            SetLineRendererPosition(shootPoint.position);
            for (int i = 0; i < numShots; i++)
            { 
                InternalShot();
                SetLineRendererPosition(hit.point);
                SetLineRendererPosition(shootPoint.position);
            }
            StartCoroutine(StartRecharge());
        }
    }

    private IEnumerator StartRecharge()
    {
        ShotAllowed = false;
        index = 0;
        yield return new WaitForSeconds(rechargeTime);
        ShotAllowed = true;
    }

    private void SetLineRendererPosition(Vector3 pos)
    {
        lineRenderer.SetPosition(index, pos);
        index++;
    }

}
