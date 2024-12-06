using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    bool dead;

    GameManager gameManager;
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
    private Animator animator;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 2f;
    public float alertSpeed = 1f;
    public float chaseSpeed = 3f;
    public string nextLevel;
    public int life;

    private NavMeshAgent agent;
    public LayerMask obstacleMask;
    float distanceToPlayer;

    private VRInvisibility vRInvisibility;

    void Start()
    {
        currentState = EnemyState.Idle;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        vRInvisibility = FindObjectOfType<VRInvisibility>();

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }

        life = 3;

        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        if (!dead)
        {
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

                case EnemyState.Chasing:
                    Chase();
                    break;
            }
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
            agent.SetDestination(patrolPoints[currentPatrolIndex].position /* * gameManager.GameTime?*/);
            animator.Play("Sword And Shield Walk");
        }
    }

    void Alerted()
    {
        Debug.Log("Enemy is alerted.");
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= alertedRange)
        {
            currentState = EnemyState.Alerted;
        }
        else if (!IsPlayerInSight())
        {
            currentState = EnemyState.Idle;
        }
    }

    void Chase()
    {
        Debug.Log("Enemy is chasing the player.");
        agent.SetDestination(player.position /* gameManager.GameTime*/);

        if (!IsPlayerInSight())
        {
            currentState = EnemyState.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevel);
            dead = true;
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
        else
        {
            currentState = EnemyState.Idle;
        }

        return false;
    }

    public void BossMuerto(string escena)
    {
        SceneManager.LoadScene(escena);
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
