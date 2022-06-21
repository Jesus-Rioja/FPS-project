using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigateRoute : MonoBehaviour
{
    public Transform route;
    [SerializeField] int currentWayPointIndex = 0;

    NavMeshAgent agent;
    [SerializeField] float reachingDistance = 2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPoint = route.GetChild(currentWayPointIndex).position;
        agent.SetDestination(currentPoint);
        if (Vector3.Distance(transform.position, currentPoint) < reachingDistance)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= route.childCount)
                { currentWayPointIndex = 0; }
        }
    }
}
