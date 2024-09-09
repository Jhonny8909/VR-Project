using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent enemy;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15f;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime; 
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        enemy = GetComponent<NavMeshAgent>();

        enemy.isStopped = false;
        enemy.speed = speedWalk;
        enemy.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Move(float speed)
    {
        enemy.isStopped = false;
        enemy.speed = speed;
    }

    void Stop()
    {
        enemy.isStopped = true;
        enemy.speed = 0;
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        enemy.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
    void CaughtPlayer()
    {
        m_PlayerInRange = true;
    }

    void LookingPlayer(Vector3 player)
    {
        enemy.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                enemy.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position,viewRadius, playerMask);
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position,player.position);

            }
        }
    }
}
