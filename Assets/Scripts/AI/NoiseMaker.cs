using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] float noiseRadius = 1f;
    [SerializeField] bool onlyWhenMoving = false;
    [SerializeField] bool silent = false;

    [SerializeField] float frecuency = 5f;

    public interface INoiseListener { void OnHeard(NoiseMaker noiseMaker); }

    private void Start()
    {
        oldPosition = transform.position;
    }

    public void MakeNoise()
    {
        InternalMakeNoise();
    }

    Vector3 oldPosition;
    float timeSinceLastNoise;
    private void Update()
    {
        if (!silent)
        {
            timeSinceLastNoise += Time.deltaTime;

            if(timeSinceLastNoise > (1f/ frecuency))
            {
                timeSinceLastNoise -= (1f / frecuency);

                if (!onlyWhenMoving || (oldPosition != transform.position))
                { InternalMakeNoise(); }

                oldPosition = transform.position;
            }
        }
    }

    void InternalMakeNoise()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, noiseRadius);

        foreach(Collider c in colliders)
        {
            INoiseListener listener = c.GetComponent<INoiseListener>();
            listener?.OnHeard(this);
        }
    }

}
