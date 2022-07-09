using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    static public Vector3 lastExplosionPosition;
    static public float lastExplosionRadius;

    [SerializeField] GameObject prefabVisualExplosion;
    [SerializeField] float radius = 1;


    void Start()
    {
        Destroy(gameObject); // DESTROY SIEMPRE OCURRE AL FINAL DEL FOTOGFRAMA

        // HACK! HACK! HACK!
        // ASUMIMOS QUE SOLO EL PLAYER TIENE UN LANZAGRANADAS
        // Y QUE EN UN FOTOGRAMA UNICAMENTE
        // PUEDE OCURRIR UNA EXPLOSION
        lastExplosionPosition = transform.position;
        lastExplosionRadius = radius;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider c in colliders)
        {
            // COMPROBAR QUE NO HAY PAREDES ENTRE LA EXPLOSION Y EL OBJETIVO
            TargetBase target = c.GetComponent<TargetBase>();
            target?.NotifyExplosion(50);
        }
        //AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
        Instantiate(prefabVisualExplosion, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(.5f, .05f);
    }
}
