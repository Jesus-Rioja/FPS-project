using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigateToTransform))]
[RequireComponent(typeof(NavigateToPosition))]
//[RequireComponent(typeof(NavigateRoute))]
public class Enemy : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable, NoiseMaker.INoiseListener
{
    [SerializeField] float attackDistance = 2f;
    [SerializeField] float attacksPerSecond = 0.5f;
    [SerializeField] float timetoForgetNoiseMaker = 1f;

    Animator animator;
    WeaponMelee meleeWeapon;

    NavigateToTransform navigateToTransform;
    NavigateToPosition navigateToPosition;
    //NavigateRoute navigateRoute;


    Transform currentTarget = null;
    NoiseMaker lastHeardNoiseMaker = null;

    Sight sight;

    enum State
    {
        Patrol,
        Idle,
        Seek,
        Attack,
        Die,
        CheckLastPosition,
    }

    [SerializeField] State state = State.Seek;

    private void Awake()
    {
        navigateToTransform = GetComponent<NavigateToTransform>();
        navigateToPosition = GetComponent<NavigateToPosition>();
        //navigateRoute = GetComponent<NavigateRoute>();

        animator = GetComponentInChildren<Animator>();
        meleeWeapon = GetComponent<WeaponMelee>();
        sight = GetComponent<Sight>();
    }

    private void Start()
    {
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = false;
        //navigateRoute.enabled = false;
    }

    float timeForNextAttack = 0f;
    float timeLeftToForgetNoiseMaker;
    Vector3 lastNoticedPosition;
    float checkPositionThreshold;

    void Update()
    {
        timeLeftToForgetNoiseMaker -= Time.deltaTime;

        if(timeLeftToForgetNoiseMaker <= 0f) { lastHeardNoiseMaker = null; }

        currentTarget =
            sight.targetInSight.Count > 0 ? sight.targetInSight[0].transform :
            lastHeardNoiseMaker != null ? lastHeardNoiseMaker.transform :
            null;

        if(currentTarget != null) { lastNoticedPosition = currentTarget.position; }
        Debug.Log(currentTarget);

        switch(state)
        {
            case State.Idle:
                if(currentTarget != null)
                    { state = State.Seek; }
                break;

            case State.Patrol:
                if(currentTarget != null)
                    { state = State.Seek; }
                break;

            case State.Seek:
                if(currentTarget != null)
                {
                    //navigateRoute.enabled = true;
                    state = State.CheckLastPosition;
                }
                else
                {
                    Seek(currentTarget);
                    if (IsInAttackRange())
                    {
                        state = State.Attack;
                        timeForNextAttack = 1f / attacksPerSecond;
                        navigateToTransform.transformGoTo = null;
                    }
                }
                break;

            case State.Attack:
                animator.SetBool("Attacking", true);
                timeForNextAttack -= Time.deltaTime;
                if(timeForNextAttack < 0f)
                {
                    timeForNextAttack += 1f / attacksPerSecond;
                    meleeWeapon.Swing();
                }
                if(!IsInAttackRange())
                {
                    state = State.Seek;
                    animator.SetBool("Attacking", false);
                    Seek(currentTarget);
                }
                break;

            case State.CheckLastPosition:
                Goto(lastNoticedPosition);
                if(Vector3.Distance(navigateToPosition.position, transform.position) < checkPositionThreshold)
                {
                    if(navigateRoute.route != null)
                    {
                        Patrol();
                        state = State.Patrol;
                    }
                    else { state = State.Idle; }
                }
                break;

            case State.Die:
                break;
        }

    }

    void GoTo(Vector3 position)
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = true;
        navigateToPosition.position = position;
    }

    void Patrol()
    {
        navigateRoute.enabled = true;
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = false;
    }

    void Seek(Transform seekTarget)
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = true;
        navigateToTransform.transformGoTo = seekTarget;
        navigateToPosition.enabled = false;
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
            navigateToTransform.transformGoTo = null;

            Collider collider = GetComponent<Collider>();
            if(collider) { collider.enabled = false; }

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody) { rigidbody.isKinematic = true; }

            animator.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavigateToTransform>().enabled = false;

            Invoke("Death", 10f);
        }
    }

    void NoiseMaker.INoiseListener.OnHeard(NoiseMaker noiseMaker)
    {
        Debug.Log(noiseMaker);
        lastHeardNoiseMaker = noiseMaker;
        timeLeftToForgetNoiseMaker = timetoForgetNoiseMaker;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
