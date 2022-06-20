using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    Enemy[] enemies;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = GetComponentsInChildren<Enemy>();
    }

    internal void DeactivateAllEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    internal void ActivateAllEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    internal bool AreAllEnemiesDead()
    {
        bool allEnemiesAreDead = true;

        for(int i = 0; allEnemiesAreDead && (i < enemies.Length); i++)
        {
            allEnemiesAreDead = enemies[i] == null;
        }

        return allEnemiesAreDead;
    }
}
