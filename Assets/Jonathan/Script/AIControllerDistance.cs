using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIControllerDistance : MonoBehaviour
{
    bool dead;
    GameManager gameManager;
    public enum EnemyState
    {
        Idle,
        Alerted,
        Attack
    }

    public EnemyState currentState;
    public Transform player;
    public float detectionRange = 10f;
    public float fieldOfViewAngle = 60f;
    public float attackRange = 5f;
    private Animator animator;
    public GameObject prefabArrow;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 2f;
    public float alertSpeed = 1f;

    private NavMeshAgent agent;
    public LayerMask obstacleMask;
    public LayerMask playerMask; // Para reconocer al jugador con el raycast

    float distanceToPlayer;

    private VRInvisibility vRInvisibility;

    float cooldown = 0f;

    void Start()
    {
        currentState = EnemyState.Idle;
        agent = GetComponent<NavMeshAgent>();
        vRInvisibility = FindObjectOfType<VRInvisibility>();
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    void Update()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

         distanceToPlayer = Vector3.Distance(transform.position, player.position);

            switch (currentState)
            {
                case EnemyState.Idle:
                    Patrol();
                    if (IsPlayerInSight())
                    {
                        currentState = EnemyState.Alerted;
                    }
                    break;

                case EnemyState.Alerted:
                    Alerted();
                    break;

                case EnemyState.Attack:

                if (cooldown <= 0f)
                {
                    Attack();
                }
                    break;
            }
    }

    

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position /* gameManager.GameTime*/);
            animator.Play("Walk");
        }
    }

    void Alerted()
    {
        Debug.Log("Enemy is alerted.");
        agent.SetDestination(player.position /* gameManager.GameTime*/);
        animator.Play("Walk");
        
        if(distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Idle;
        }

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (!IsPlayerInSight())
        {
            currentState = EnemyState.Idle;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy is attempting to attack the player.");

        agent.isStopped = true;
        animator.Play("Attack");
        if(cooldown <= 0f) cooldown = 1f;
        Instantiate(prefabArrow, gameObject.transform);

        if (distanceToPlayer > attackRange)
        {
            Debug.Log("El jugador salio del rango");
            agent.isStopped = false;
            currentState = EnemyState.Alerted;
        }

    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (vRInvisibility.IsInvisible == false)
        {
            if (distanceToPlayer <= detectionRange)
            {
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                if (angleToPlayer <= fieldOfViewAngle / 2)
                {
                    if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 rightLimit = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward * detectionRange;
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, rightLimit);
        Gizmos.DrawRay(transform.position, leftLimit);
    }
}