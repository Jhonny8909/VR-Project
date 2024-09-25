using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float fieldOfViewAngle = 90f;
    public float rotationSpeed = 2f;
    public float raycastRange = 20f;

    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public LineRenderer lineRenderer;
    public Transform Camera;

    private bool playerDetected = false;
    private float rotationAmount;

    void Start()
    {
        lineRenderer.positionCount = 4;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void Update()
    {
        RotateTurret();

        UpdateLineRenderer();

        if (IsPlayerInSight())
        {
            Debug.Log("Player detected, attempting to shoot!");
            FireRaycast();
        }
    }

    void RotateTurret()
    {
        rotationAmount = Mathf.PingPong(Time.time * rotationSpeed, fieldOfViewAngle) - (fieldOfViewAngle / 2);
        transform.rotation = Quaternion.Euler(30f, rotationAmount, transform.rotation.z);
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
                    playerDetected = true;
                    return true;
                }
            }
        }

        playerDetected = false;
        return false;
    }

    void FireRaycast()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, raycastRange, obstacleMask | playerMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Player hit by turret!");
                KillPlayer();
            }
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player has been killed by the turret.");
    }

    void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, Camera.position);

        Vector3 rightDirection = DirectionFromAngle(fieldOfViewAngle / 2);
        Vector3 leftDirection = DirectionFromAngle(-fieldOfViewAngle / 2);

        lineRenderer.SetPosition(1, Camera.position + rightDirection * detectionRange);
        lineRenderer.SetPosition(2, Camera.position + leftDirection * detectionRange);

        lineRenderer.SetPosition(3, Camera.position);
    }

    Vector3 DirectionFromAngle(float angleInDegrees)
    {
        return Quaternion.Euler(0, angleInDegrees + Camera.eulerAngles.y, 0) * Camera.forward;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Camera.position, detectionRange);

        Vector3 rightLimit = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * Camera.forward * detectionRange;
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * Camera.forward * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Camera.position, rightLimit);
        Gizmos.DrawRay(Camera.position, leftLimit);
    }
}