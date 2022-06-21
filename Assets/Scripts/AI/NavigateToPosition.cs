using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigateToPosition : MonoBehaviour
{
    public Vector3 position;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(position);
    }
}
