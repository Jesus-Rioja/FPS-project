using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Enemies;
    [SerializeField] Transform[] spawnPoints;

    Round round;

    float timeToNextEnemy = 5f;

    private void Awake()
    {
        round = GetComponentInParent<Round>();
        Debug.Log("He pillado: " + round.name);
    }

    void Update()
    {
        timeToNextEnemy -= Time.deltaTime;

        if (timeToNextEnemy <= 0)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        for(int i = 0; i <= Random.Range(1, 6); i++)
        {
            Instantiate(Enemies[Random.Range(0, Enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, round.transform);
        }
        round.gameObject.GetComponent<Round>().CheckEnemies();

        timeToNextEnemy = Random.Range(2f, 6f);
    }
}
