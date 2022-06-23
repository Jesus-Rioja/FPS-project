using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigateToTransform))]
[RequireComponent(typeof(NavigateToPosition))]
[RequireComponent(typeof(NavigateRoute))]
public class Enemy : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable, NoiseMaker.INoiseListener
{
    [SerializeField] float attacksPerSecond = 0.5f;
    [SerializeField] float timetoForgetNoiseMaker = 1f;

    Animator animator;

    WeaponBase currentWeapon;

    NavigateToTransform navigateToTransform;
    NavigateToPosition navigateToPosition;
    NavigateRoute navigateRoute;

    [Header("Cover parameters")]
    [SerializeField] float fearDistance = 15f;
    [SerializeField] float coverSearchRadius = 20f;
    [SerializeField] LayerMask coverSearchLayerMask; //= 1 << LayerMask.NameToLayer("CoverPoints"); LAYER SON UNA CADENA DE 32 BITS, CADA BIT  A1 REPRESENTA UN LAYER, CON ETSA OPERACION DESPLAZAMOS UN 1 30 VECES A LA IZQ (30 = LAYER COVERPOINTS)
    [SerializeField] LayerMask occludingLayerMask = Physics.DefaultRaycastLayers;

    Transform currentTarget = null;
    NoiseMaker lastHeardNoiseMaker = null;

    Sight sight;

    enum State
    {
        Patrol,
        Idle,
        Seek,
        TakeCover,
        Attack,
        Die,
        CheckLastPosition,
    };

    public enum BehaviourType
    {
        Cautious,
        Valiant,
        Sneaky,
        Guardian,
    }

    [SerializeField] BehaviourType behaviourType = BehaviourType.Valiant;
    [SerializeField] State state = State.Seek;
    [SerializeField] float checkPositionThreshold = 1.5f;

    private void Awake()
    {
        navigateToTransform = GetComponent<NavigateToTransform>();
        navigateToPosition = GetComponent<NavigateToPosition>();
        navigateRoute = GetComponent<NavigateRoute>();

        animator = GetComponentInChildren<Animator>();
        currentWeapon = GetComponent<WeaponBase>();
        sight = GetComponent<Sight>();
    }

    private void Start()
    {
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = false;
        navigateRoute.enabled = false;

        state = State.Idle;
        if (behaviourType == BehaviourType.Guardian) 
        { 
            GetComponent<NavMeshAgent>().speed = 0f; 

        }
        else if(navigateRoute.route != null)
        {
            state = State.Patrol;
        }
    }

    float timeForNextAttack = 0f;
    float timeLeftToForgetNoiseMaker;
    Vector3 lastNoticedPosition;
    bool locateFirstTarget = false;

    void Update()
    {
        UpdateNoiseMaker();

        UpdateCurrentTarget();

        switch (state)
        {
            case State.Idle:
                if (currentTarget != null)
                    { state = State.Seek; }
                else 
                    { GoTo(transform.position); }
                break;

            case State.Patrol:
                Patrol();
                if (currentTarget != null)
                { state = State.Seek; }
                break;

            case State.Seek:
                if (currentTarget != null)
                {
                    navigateRoute.enabled = true;
                    state = State.CheckLastPosition;
                }
                else
                {
                    GoTo(currentTarget);
                    if (IsInAttackRange())
                    {
                        state = State.Attack;
                        timeForNextAttack = 1f / attacksPerSecond;
                        navigateToTransform.transformGoTo = null;
                    }
                }
                break;

            case State.Attack: //opcional ataque hacia arriba/abajo de los enemigos
                UpdateAttack();
                break;

            case State.TakeCover:
                UpdateTakeCover();
                break;

            case State.CheckLastPosition:
                if (currentTarget != null)
                {
                    state = State.Seek;
                }
                else
                {
                    GoTo(lastNoticedPosition);
                    if (Vector3.Distance(navigateToPosition.position, transform.position) < checkPositionThreshold)
                    {
                        if (navigateRoute.route != null)
                        {
                            Patrol();
                            state = State.Patrol;
                        }
                        else { state = State.Idle; }
                    }
                }
                break;

            case State.Die:
                break;
        }

    }

    bool currentSideStepDirection = false;
    bool oldIsInMinRange = false;

    void UpdateAttack()
    {
        if (currentTarget == null)
        {
            state = State.CheckLastPosition;
        }
        else
        {
            //animator.SetBool("Attacking", true);

            bool advanceWhileAttacking = behaviourType == BehaviourType.Valiant;
            bool isInMinRange = Vector3.Distance(currentTarget.position, transform.position) < currentWeapon.GetMinRange();

            if (advanceWhileAttacking)
            { 
                if(!isInMinRange)
                {
                    GoTo(currentTarget);
                }
                else
                {
                    if(oldIsInMinRange != isInMinRange)
                        { currentSideStepDirection = Random.Range(0f, 100f) < 50f; }
                    SideStep(currentSideStepDirection);
                }
            }
            else if ((Vector3.Distance(currentTarget.position, transform.position) < fearDistance) && (behaviourType == BehaviourType.Cautious))
            {
                selectedCover = FindBestCover();
                if (selectedCover)
                    state = State.TakeCover;
                else
                    GoTo(transform.position);
            }
            else
                { GoTo(transform.position); }


            //Aiming / Shooting
            LookAt(currentTarget);

            // TODO: chequear el tipo de arma, y
            // utilizar las llamadas correctas
            // para disparar

            currentWeapon.Shot();

            if(currentWeapon.NeedsReload())
                { currentWeapon.Reload(); }

            if (!IsInAttackRange())
            {
                state = State.Seek;
                //animator.SetBool("Attacking", false);
                GoTo(currentTarget);
            }

            oldIsInMinRange = isInMinRange;
        }
    }

    Transform selectedCover;
    float timeCovering;
    [SerializeField] float thresholdCover = 1f;

    void UpdateTakeCover()  //OPCIONAL CURRARSELO MAS
    {
        if (Vector3.Distance(selectedCover.position, transform.position) > thresholdCover)
        { 
            //Yendo a cubrirse
            GoTo(selectedCover); 
        }
        else
        {
            //Estamos a cubierto
            if (currentTarget != null)
            {
                selectedCover = FindBestCover();
                if(!selectedCover)
                    { state = State.Attack; }
            }
            else
            {
                timeCovering -= Time.deltaTime;
                if (timeCovering < 0f)
                { state = State.CheckLastPosition; }
            }

        }
    }

    void UpdateNoiseMaker()
    {
        timeLeftToForgetNoiseMaker -= Time.deltaTime;

        if (timeLeftToForgetNoiseMaker <= 0f) { lastHeardNoiseMaker = null; }
    }

    void UpdateCurrentTarget()
    {
        currentTarget = null;
        if (sight.targetInSight.Count > 0)
        { currentTarget = sight.targetInSight[0].transform; }
        else
        {
            if ((behaviourType != BehaviourType.Sneaky) || locateFirstTarget)
            {
                if (lastHeardNoiseMaker != null) { currentTarget = lastHeardNoiseMaker.transform; }
            }
        }

        locateFirstTarget |= currentTarget != null;

        if (currentTarget != null) { lastNoticedPosition = currentTarget.position; }

        Debug.Log(currentTarget);
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

    void GoTo(Transform targetTransform)
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = true;
        navigateToTransform.transformGoTo = targetTransform;
        navigateToPosition.enabled = false;
    }

    void LookAt(Transform lookTarget)
    {
        Vector3 positionOnSameHigh = lookTarget.position;
        positionOnSameHigh.y = transform.position.y;

        transform.LookAt(positionOnSameHigh);
    }

    void SideStep(bool toRight)
    {
        Vector3 destination = transform.position + (toRight ? Vector3.right : -Vector3.right);
        GoTo(destination);   
    }

    bool IsInAttackRange()
    {
        if (PlayerMovement.instance != null)
            return Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < currentWeapon.GetMaxRange();
        else
            return false;
    }

    Transform FindBestCover()
    {
        Collider[] potentialCovers = Physics.OverlapSphere(transform.position, coverSearchRadius, coverSearchLayerMask, QueryTriggerInteraction.Ignore);

        //TODO: discard covers that are closer to
        //      the current target than this entity
        //TODO: sort potential covers
        foreach (Collider c in potentialCovers)
        {
            RaycastHit hit;
            Vector3 direction = c.transform.position - currentTarget.position;
            if (Physics.Raycast(currentTarget.position, direction, out hit, direction.magnitude, occludingLayerMask, QueryTriggerInteraction.Ignore))
            {
                return c.transform;
            }
        }

        return null;
    }



    void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if (state != State.Die)
        {
            state = State.Die;
            navigateToTransform.transformGoTo = null;

            Collider collider = GetComponent<Collider>();
            if (collider) { collider.enabled = false; }

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
