using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateToTransform : MonoBehaviour
{
    public Transform transformGoTo;

    NavMeshAgent navMeshAgent;
    Animator animator;
    
    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        transformGoTo = PlayerMovement.instance.transform;
    }

    void Update()
    {
        if (navMeshAgent.velocity != Vector3.zero)
            animator.SetBool("Running", true);
        else
            animator.SetBool("Running", false);

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
