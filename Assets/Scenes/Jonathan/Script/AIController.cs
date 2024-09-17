using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
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
    public float alertedRange = 5f;         
    public float chaseRange = 2f;           
    public float stopDistance = 1.5f;

    private NavMeshAgent agent;             
    private Rigidbody rb;                   
    private bool playerDetected = false;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.SetDestination(player.transform.position);
    }

    /*void Start()
    {
        currentState = EnemyState.Idle;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updatePosition = false;
        agent.updateRotation = true;
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
        Debug.Log("Enemy is idle.");
        if (!playerDetected && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            playerDetected = true;
            currentState = EnemyState.Alerted;
        }
    }

    void Alerted()
    {
        Debug.Log("Enemy is alerted.");

        if (Vector3.Distance(transform.position, player.position) <= alertedRange)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            agent.SetDestination(player.position);

            MoveAgent();
        }

        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            playerDetected = false;
            currentState = EnemyState.Idle;
        }
    }

    void Chase()
    {
        Debug.Log("Enemy is chasing the player.");

        if (Vector3.Distance(transform.position, player.position) <= chaseRange)
        {
            agent.SetDestination(player.position);

            MoveAgent();
        }

        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            playerDetected = false;  
            currentState = EnemyState.Idle;
        }
    }

    void MoveAgent()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 velocity = player.transform.position;
            agent.SetDestination(velocity);
        }
        else
        {
            rb.velocity = Vector3.zero;  
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertedRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}