using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 0.1f;

    void Start()
    {
        Invoke("DestroyMe", timeToDestroy);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
