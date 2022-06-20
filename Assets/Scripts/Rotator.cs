using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotation = new Vector3(1f, 0.6f, 0.3f);
    [SerializeField] float speed = 20;

    void Update()
    {
        transform.Rotate(rotation * speed * Time.deltaTime);
    }
}
