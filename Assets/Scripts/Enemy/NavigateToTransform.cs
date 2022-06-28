using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateToTransform : MonoBehaviour
{
    public Transform transformGoTo;

    NavMeshAgent navMeshAgent;
    
    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        transformGoTo = PlayerMovement.instance.transform;
    }

    void Update()
    {
        if (navMeshAgent.enabled)
        {
            if (transformGoTo)
            {
                navMeshAgent.SetDestination(transformGoTo.position);
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }
        }


        FaceTarget();
    }

    void FaceTarget()
    {
        var turnToTarget = transform.position;

        if (PlayerMovement.instance != null)
            turnToTarget = PlayerMovement.instance.transform.position;

        Vector3 direction = (turnToTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

}
