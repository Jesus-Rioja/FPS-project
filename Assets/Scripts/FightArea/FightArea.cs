using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour
{
    [SerializeField] Transform limits;
    Round[] rounds;
    AudioSource[] limitSounds;
    int currentRound = -1;

    void Awake()
    {
        rounds = GetComponentsInChildren<Round>();
        limitSounds = GetComponents<AudioSource>();
    }

    private void Start()
    {
        foreach (Round r in rounds) { r.DeactivateAllEnemies(); }
        limits.gameObject.SetActive(false);
    }

    void Update()
    {
        if((currentRound >= 0) && (currentRound < rounds.Length))
        {
            if (rounds[currentRound].AreAllEnemiesDead())
            {
                currentRound++;

                if (currentRound < rounds.Length) { Invoke("ActivateNextRound", 3f); }
                else
                {
                    limitSounds[1].Play();
                    limits.gameObject.SetActive(false); 
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && (currentRound < 0))
        {
            Destroy(GetComponent<Rigidbody>());
            currentRound = 0;
            rounds[currentRound].ActivateAllEnemies();
            limits.gameObject.SetActive(true);
            limitSounds[0].Play();
        }
    }

    private void ActivateNextRound()
    {
        rounds[currentRound].ActivateAllEnemies();
    }
}
