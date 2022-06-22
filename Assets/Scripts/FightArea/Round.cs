using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Round : MonoBehaviour
{
    public Enemy[] enemies;

    void Awake()
    {
        CheckEnemies();
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
        //EnemiesLeftEvent.Invoke(enemies.Length);
    }

    internal bool AreAllEnemiesDead()
    {
        bool allEnemiesAreDead = true;

        for (int i = 0; allEnemiesAreDead && (i < enemies.Length); i++)
        {
            //allEnemiesAreDead = enemies[i].GetComponent<TargetWithLife>().Life <= 0;
            if(enemies[i] != null && enemies[i].GetComponent<TargetWithLife>().Life > 0) { allEnemiesAreDead = false; }
        }

        return allEnemiesAreDead;
    }

    public void CheckEnemies()
    {
        enemies = GetComponentsInChildren<Enemy>();
    }

}
