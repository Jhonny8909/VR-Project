using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool playerDetected = false;
    private float rotationAmount;
    private Quaternion initialRotation;

    public Transform turretCamera;

    private VRInvisibility vRInvisibility;

    void Start()
    {
        initialRotation = transform.rotation;

        lineRenderer.positionCount = 4;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        vRInvisibility = FindObjectOfType<VRInvisibility>();
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
        float rotationOffset = Mathf.PingPong(Time.time * rotationSpeed, fieldOfViewAngle) - (fieldOfViewAngle / 2);
        transform.rotation = initialRotation * Quaternion.Euler(0, rotationOffset, 0);
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!vRInvisibility.IsInvisible)
        {
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
        SceneManager.LoadScene("GrayBox2.0");
    }

    void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, turretCamera.position);

        Vector3 rightDirection = DirectionFromAngle(fieldOfViewAngle / 2);
        Vector3 leftDirection = DirectionFromAngle(-fieldOfViewAngle / 2);

        lineRenderer.SetPosition(1, turretCamera.position + rightDirection * detectionRange);
        lineRenderer.SetPosition(2, turretCamera.position + leftDirection * detectionRange);

        // Cerrar el triángulo
        lineRenderer.SetPosition(3, turretCamera.position);
    }

    Vector3 DirectionFromAngle(float angleInDegrees)
    {
        return Quaternion.Euler(turretCamera.eulerAngles.x, angleInDegrees + turretCamera.eulerAngles.y, 0) * Vector3.forward;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(turretCamera.position, detectionRange);

        Vector3 rightLimit = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * turretCamera.forward * detectionRange;
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * turretCamera.forward * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(turretCamera.position, rightLimit);
        Gizmos.DrawRay(turretCamera.position, leftLimit);
    }
}