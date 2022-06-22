using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEndLevel : MonoBehaviour
{
    public UnityEvent LevelEndEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("UnlockLevel2", 1);
            LevelEndEvent.Invoke();
        }
    }
}
