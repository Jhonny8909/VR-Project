using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;  
    public LayerMask detectionLayers; 
    private VRInvisibility playerInvisibility;
    private bool playerDetected = false;

    void Start()
    {
        playerInvisibility = player.GetComponent<VRInvisibility>();
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if (playerInvisibility.IsInvisible)
        {
            if (playerDetected)
            {
                Debug.Log("El jugador se volvió invisible y ya no puede ser detectado.");
                playerDetected = false;
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.red);

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange, detectionLayers))
            {
                if (hit.transform == player)
                {
                    if (!playerDetected)
                    {
                        Debug.Log("Jugador detectado por el enemigo.");
                        playerDetected = true;
                    }
                }
                else
                {
                    if (playerDetected)
                    {
                        Debug.Log("El jugador ya no está a la vista del enemigo, algo lo bloquea.");
                        playerDetected = false;
                    }
                }
            }
        }
        else if (playerDetected)
        {
            Debug.Log("Jugador fuera del rango de detección.");
            playerDetected = false;
        }
    }
}
