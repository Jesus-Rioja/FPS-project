using System;
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

    [SerializeField] int weaponSelectedIndex = 0;
    WeaponBase[] avaiableWeapons;
    WeaponBase currentWeapon;

    Animator animator;

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

    [Header("On death drops")]
    [SerializeField] GameObject AmmoDrop;

    Sight sight;

    Vector3 spawnPosition;
    Quaternion spawnRotation;

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
        avaiableWeapons = GetComponentsInChildren<WeaponBase>();
        sight = GetComponent<Sight>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Start()
    {
        for(int i = 0; i < avaiableWeapons.Length; i++)
        {
            if (weaponSelectedIndex == i)
            {
                avaiableWeapons[i].gameObject.SetActive(true);
                currentWeapon = avaiableWeapons[i];
            }
            else 
            {
                avaiableWeapons[i].gameObject.SetActive(false);
            }

        }

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

        if (currentWeapon.GetUseType() == WeaponBase.WeaponUseType.ContinuousShot && state != State.Attack)
        {
            currentWeapon.StopShooting();
        }

        switch (state)
        {
            case State.Idle:
                if (currentTarget != null)
                    { state = State.Seek; }
                else 
                    { GoTo(transform.position); }
                break;

            case State.Patrol:
                if (currentTarget != null)
                    { state = State.Seek; }
                else
                    { Patrol(); }
                break;

            case State.Seek:
                if (currentTarget == null)
                {
                    navigateRoute.enabled = false;
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
                        { currentSideStepDirection = UnityEngine.Random.Range(0f, 100f) < 50f; }
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

            if (currentWeapon.GetUseType() == WeaponBase.WeaponUseType.Shot)
            {
                Debug.Log("Dispario una");
                currentWeapon.Shot();
            }
            else if (currentWeapon.GetUseType() == WeaponBase.WeaponUseType.ContinuousShot)
            {
                Debug.Log("Dispario multi");
                currentWeapon.StartShooting();
            }


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
    float timeCovering = 0f;
    [SerializeField] float thresholdCover = 2f;

    void UpdateTakeCover()  //OPCIONAL CURRARSELO MAS
    {
        if (Vector3.Distance(selectedCover.position, transform.position) > thresholdCover)
        {
            animator.SetBool("Kneel", false);
            //Yendo a cubrirse
            GoTo(selectedCover); 
        }
        else
        {
            animator.SetBool("Kneel", true);
            //Estamos a cubierto
            if (currentTarget != null)
            {
                
                selectedCover = FindBestCover();
                if(!selectedCover)
                {
                    animator.SetBool("Kneel", false);
                    state = State.Attack; 
                }
            }
            else
            {
                timeCovering -= Time.deltaTime;
                if (timeCovering < 0f)
                {
                    animator.SetBool("Kneel", false);
                    state = State.CheckLastPosition;
                    timeCovering = 2f;
                }
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
        if (sight.collidersInSight.Count > 0)
        { currentTarget = sight.collidersInSight[0].transform; }
        else
        {
            if ((behaviourType != BehaviourType.Sneaky) || locateFirstTarget)
            {
                if (lastHeardNoiseMaker != null) { currentTarget = lastHeardNoiseMaker.transform; }
            }
        }

        locateFirstTarget |= currentTarget != null;

        if (currentTarget != null) { lastNoticedPosition = currentTarget.position; }
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

        Collider[] sortedPotentialCovers = SortCovers(potentialCovers);

        if (sortedPotentialCovers == null)
            return null;

        float bestDistance = FindBestDistance(sortedPotentialCovers);
        //TODO: discard covers that are closer to
        //      the current target than this entity
        //TODO: sort potential covers

        Transform bestAxis = null;

        foreach (Collider c in sortedPotentialCovers)
        {
            float coverDistance = Vector3.Distance(transform.position, c.transform.position);
            if(coverDistance == bestDistance)
            {
                Transform[] coverAxis = c.GetComponentsInChildren<Transform>();
                bestAxis = coverAxis[0];

                for (int i = 1; i < coverAxis.Length; i++)
                {
                    float farestDistance = Vector3.Distance(currentTarget.position, bestAxis.transform.position);
                    float newDistance = Vector3.Distance(currentTarget.position, coverAxis[i].transform.position);
                    if (farestDistance < newDistance)
                    { bestAxis = coverAxis[i]; }
                }
            }
        }

        return bestAxis;
    }

    Collider[] SortCovers(Collider[] coversArray)
    {
        bool found = false;
        Collider[] sortedCoversArray = new Collider[coversArray.Length];

        int j = 0;
        for(int i = 0; i < coversArray.Length; i++)
        {
            float enemyToCoverDistance = Vector3.Distance(transform.position, coversArray[i].transform.position);
            float playerToCoverDistance = Vector3.Distance(currentTarget.position, coversArray[i].transform.position);
            if (enemyToCoverDistance < playerToCoverDistance)
            {
                RaycastHit hit;
                Vector3 direction = coversArray[i].transform.position - currentTarget.position;
                if (Physics.Raycast(currentTarget.position, direction, out hit, direction.magnitude, occludingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    found = true;
                    sortedCoversArray[j] = coversArray[i];
                    j++;
                }
            }
        }

        Array.Resize<Collider>(ref sortedCoversArray, j);

        if (found)
            return sortedCoversArray;
        else
            return null;
    }

    float FindBestDistance(Collider[] coversArray)
    {
        float nearestDistance = Vector3.Distance(transform.position, coversArray[0].transform.position);

        for (int i = 1; i < coversArray.Length; i++)
        {
            float newDistance = Vector3.Distance(transform.position, coversArray[i].transform.position);
            if (nearestDistance > newDistance)
            { nearestDistance = newDistance; }
        }

        return nearestDistance;
    }


    void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if (state != State.Die)
        {
            state = State.Die;
            navigateToTransform.transformGoTo = null;
            navigateToPosition.position = transform.position;
            navigateRoute.route = null;

            Collider collider = GetComponent<Collider>();
            if (collider) { collider.enabled = false; }

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody) { rigidbody.isKinematic = true; }

            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavigateToTransform>().enabled = false;
            GetComponent<NavigateToPosition>().enabled = false;
            GetComponentInChildren<Animator>().enabled = false;

            Instantiate(AmmoDrop, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);

            Invoke("Death", 10f);
        }
    }

    void NoiseMaker.INoiseListener.OnHeard(NoiseMaker noiseMaker)
    {
        lastHeardNoiseMaker = noiseMaker;
        timeLeftToForgetNoiseMaker = timetoForgetNoiseMaker;
    }

    public void locateEnemy()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
