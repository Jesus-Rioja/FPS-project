using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeactivator : MonoBehaviour
{
    FightArea fightArea;

    private void Awake()
    {
        fightArea = transform.parent.parent.parent.GetComponent<FightArea>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fightArea.DeactivateEnemies();
            //limitSounds[0].Play();
        }
    }
}
