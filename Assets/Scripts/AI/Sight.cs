using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sight : MonoBehaviour
{
    [SerializeField] float sightDistance = 10f;
    [SerializeField] float sightWidth = 10f;
    [SerializeField] float sightHeight = 5f;
    [SerializeField] LayerMask targetLayerMask = Physics.DefaultRaycastLayers;
    [SerializeField] LayerMask occludingLayerMask = Physics.DefaultRaycastLayers;
    [SerializeField] string[] targetTags = { "Player" };

    public List<Collider> collidersInSight;

    void Update()
    {
        // Posible mejora: hacer esta comprobación
        // cada cierto tiempo
        Collider[] collidersInBox =
            Physics.OverlapBox(
                transform.position + (transform.forward * sightDistance / 2f),
                new Vector3(sightWidth / 2f, sightHeight / 2f, sightDistance / 2f),
                transform.rotation,
                targetLayerMask,
                QueryTriggerInteraction.Ignore);

        collidersInSight.Clear();

        foreach (Collider c in collidersInBox)
        {
            if (targetTags.Contains(c.tag))
            {
                Vector3 direction = c.transform.position - transform.position;
                if (!Physics.Raycast(transform.position, direction, direction.magnitude, occludingLayerMask, QueryTriggerInteraction.Ignore))
                { collidersInSight.Add(c); }
            }
        }
    }
}
