using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour
{
    [SerializeField] Transform limits;
    Round[] rounds;
    int currentRound = -1;
    // Start is called before the first frame update
    void Awake()
    {
        rounds = GetComponentsInChildren<Round>();
    }

    private void Start()
    {
        foreach (Round r in rounds) { r.DeactivateAllEnemies(); }
        limits.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((currentRound >= 0) && (currentRound < rounds.Length))
        {
            if (rounds[currentRound].AreAllEnemiesDead())
            {
                currentRound++;

                if (currentRound < rounds.Length) { rounds[currentRound].ActivateAllEnemies(); }
                else { limits.gameObject.SetActive(false); }
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
        }
    }
}
