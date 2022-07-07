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
        if(!AreAllEnemiesDead())
        {
            foreach (Enemy e in enemies)
            {
                e.gameObject.SetActive(true);
                e.GetComponent<TargetWithLifeThatNotifies>().regenerateLife();
            }
        }
    }

    public void DeactivateEnemies()
    {
        if (!AreAllEnemiesDead())
        {
            foreach (Enemy e in enemies)
            {
                e.locateEnemy();
                e.gameObject.SetActive(false);
            }
        }
    }

    internal bool AreAllEnemiesDead()
    {
        bool allEnemiesAreDead = true;

        for (int i = 0; allEnemiesAreDead && (i < enemies.Length); i++)
        {
            if (enemies[i] != null && enemies[i].GetComponent<TargetWithLife>().Life > 0) { allEnemiesAreDead = false; }
        }

        return allEnemiesAreDead;
    }
}
