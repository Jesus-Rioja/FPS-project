using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    static public Vector3 lastExplosionPosition;
    static public float lastExplosionRadius;

    [SerializeField] GameObject prefabVisualExplosion;
    [SerializeField] float radius = 1;
    [SerializeField] AudioClip explosionAudioClip; // CON AUDIOCLIP NO PODEMOS METER EL SONIDO EN EL AUDIOMIXER, MIRAR DE CASMBIRLO CON EVENTS(LISTENER)

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
            target?.NotifyExplosion();
        }

        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
        Instantiate(prefabVisualExplosion, transform.position, Quaternion.identity);
    }
}
