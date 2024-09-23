using UnityEngine;
using UnityEngine.AI;

public class AIControllerMelee : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Alerted,
        Chasing
    }

    public EnemyState currentState;
    public Transform player;
    public float detectionRange = 10f;
    public float fieldOfViewAngle = 60f;
    public float alertedRange = 5f;
    public float chaseRange = 2f;
    public float stopDistance = 1.5f;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 2f;

    private NavMeshAgent agent;
    private bool playerDetected = false;
    public LayerMask obstacleMask;

    void Start()
    {
        currentState = EnemyState.Idle;

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Alerted:
                Alerted();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
        }
    }

    void Idle()
    {
        Debug.Log("Enemy is patrolling.");

        if (!playerDetected && IsPlayerInSight())
        {
            playerDetected = true;
            currentState = EnemyState.Alerted;
        }

        Patrol();
    }

    void Alerted()
    {
        Debug.Log("Enemy is alerted.");
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= alertedRange)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            agent.SetDestination(player.position);
            agent.speed = 1;
        }

        if (!IsPlayerInSight())
        {
            playerDetected = false;
            currentState = EnemyState.Idle;
        }
    }

    void Chase()
    {
        Debug.Log("Enemy is chasing the player.");
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            agent.SetDestination(player.position);
            agent.speed = 3;
        }

        if (!IsPlayerInSight())
        {
            playerDetected = false;
            currentState = EnemyState.Idle;
        }
    }



    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        agent.speed = patrolSpeed;
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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