using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDestroy : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsToInstantiate;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        foreach (GameObject prefab in prefabsToInstantiate)
        {
            Instantiate(prefab, transform.position, transform.rotation);
        }
    }
}
