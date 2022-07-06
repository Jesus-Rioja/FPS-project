using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour
{
    Enemy[] enemies;
    //AudioSource[] limitSounds;

    void Awake()
    {
        enemies = GetComponentsInChildren<Enemy>();
        //limitSounds = GetComponents<AudioSource>();

    }

    private void Start()
    {
        DeactivateEnemies();
    }

    public void ActivateEnemies()
    {
        foreach(Enemy e in enemies)
        {
            e.gameObject.SetActive(true);
        }
    }

    public void DeactivateEnemies()
    {
        foreach (Enemy e in enemies)
        {
            e.locateEnemy();
            e.gameObject.SetActive(false);
        }
    }
}
