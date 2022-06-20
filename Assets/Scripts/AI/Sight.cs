using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sight : MonoBehaviour
{
    [Header("Measures")]
    [SerializeField] float sightDistance = 20f;
    [SerializeField] float sightWidth = 10f;
    [SerializeField] float sightHeight = 5f;

    [Header("Target discrimination")]
    [SerializeField] LayerMask visibleLayers = Physics.DefaultRaycastLayers;
    [SerializeField] LayerMask occludingLayers = Physics.DefaultRaycastLayers;
    [SerializeField] string[] interestingTags = { "Player" };

    [Header("Update Parameters")]
    [SerializeField] float updateFrequency = 5f;

    [Header("Targets acquired")]
    public List<Collider> targetInSight = new List<Collider>();

    float timeToUpdate = 0f;

    void Update()
    {
        timeToUpdate -= Time.deltaTime;

        if(timeToUpdate <= 0f)
        {
            timeToUpdate += 1f / updateFrequency;
            //RaycastHit hit;

            Collider[] colliders = Physics.OverlapBox(
                transform.TransformPoint(Vector3.forward * sightDistance / 2),
                new Vector3(sightWidth, sightHeight, sightDistance) / 2f,
                transform.rotation,
                visibleLayers,
                QueryTriggerInteraction.Ignore);

            targetInSight.Clear();

            for(int i = 0; i < colliders.Length; i++)
            {
                if(interestingTags.Contains(colliders[i].tag))
                {
                    Vector3 direction = colliders[i].transform.position - transform.position;
                    if (!Physics.Raycast(transform.position, direction, /*out hit,*/ direction.magnitude, occludingLayers, QueryTriggerInteraction.Ignore))
                    {
                        targetInSight.Add(colliders[i]);
                        /*if(hit.transform == colliders[i].transform)
                        { targetInSight.Add(colliders[i]); }*/
                    }
                }
            }
        }
    }
}
