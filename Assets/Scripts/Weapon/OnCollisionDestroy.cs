using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionDestroy : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsToInstantiate;
    public UnityEvent ExplosionSound;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        foreach (GameObject prefab in prefabsToInstantiate)
        {
            Instantiate(prefab, transform.position, transform.rotation);
        }

        ExplosionSound.Invoke();
    }
}
