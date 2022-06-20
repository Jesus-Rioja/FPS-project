using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigateToTransform))]
public class Enemy : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable
{
    [SerializeField] float attackDistance = 2f;

    Animator animator;
    NavigateToTransform target;
    WeaponMelee meleeWeapon;

    Transform currentTarget = null;

    Sight sight;

    enum State
    {
        Patrol,
        Idle,
        Seek,
        Attack,
        Die,
    }

    [SerializeField] State state = State.Seek;

    private void Awake()
    {
        target = GetComponent<NavigateToTransform>();
        animator = GetComponentInChildren<Animator>();
        meleeWeapon = GetComponent<WeaponMelee>();
        sight = GetComponent<Sight>();
    }

    private void Start()
    {
        target.transformGoTo = PlayerMovement.instance.transform;
    }

    void Update()
    {
        currentTarget = sight.targetInSight[0].transform;

        switch(state)
        {
            case State.Seek:
                if(IsInAttackRange())
                {
                    state = State.Attack;
                    target.transformGoTo = null;
                }
                break;

            case State.Attack:
                animator.SetBool("Attacking", true);

                if(!IsInAttackRange())
                {
                    state = State.Seek;
                    animator.SetBool("Attacking", false);
                    if(PlayerMovement.instance != null)
                        target.transformGoTo = PlayerMovement.instance.transform;
                }
                break;

            case State.Die:
                break;
        }

    }

    bool IsInAttackRange()
    {
        if (PlayerMovement.instance != null)
            return Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < attackDistance;
        else
            return false;
    }

    void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if(state != State.Die)
        {
            state = State.Die;

            animator.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavigateToTransform>().enabled = false;

            Invoke("Death", 10f);
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
