using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIControllerDistance : MonoBehaviour
{
    bool dead;
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

    void Start()
    {
        currentState = EnemyState.Idle;
        agent = GetComponent<NavMeshAgent>();
        vRInvisibility = FindObjectOfType<VRInvisibility>();

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(!dead)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    Patrol();
                    if (IsPlayerInSight())
                    {
                        TransitionToState(EnemyState.Alerted);
                    }
                    break;

                case EnemyState.Alerted:
                    Alerted();
                    break;

                case EnemyState.Attack:
                    Attack();
                    break;
            }
        }
        
    }

    void TransitionToState(EnemyState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case EnemyState.Alerted:
                agent.speed = alertSpeed;
                break;
            case EnemyState.Idle:
                agent.speed = patrolSpeed;
                GoToNextPatrolPoint();
                break;
            case EnemyState.Attack:
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
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Alerted()
    {
        Debug.Log("Enemy is alerted.");

        
        if (distanceToPlayer <= attackRange)
        {
            TransitionToState(EnemyState.Attack);
        }
        else if (!IsPlayerInSight())
        {
            TransitionToState(EnemyState.Idle);
        }
    }

    void Attack()
    {
        Debug.Log("Enemy is attempting to attack the player.");

        agent.isStopped = true;
        
        KillPlayer();

        if (distanceToPlayer > attackRange)
        {
            agent.isStopped = false;
            TransitionToState(EnemyState.Idle);
        }

        
    }

    void KillPlayer()
    {
        Debug.Log("Player has been killed by the enemy.");
        SceneManager.LoadScene("GrayBox2.0");
        dead = true;
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