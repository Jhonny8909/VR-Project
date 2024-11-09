using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public string scene;
    public float throwSpeedMultiplier = 1f;
    private Vector3 playerPosition;
    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        playerPosition = FindObjectOfType<VRInvisibility>().transform.position;
        FuerzaFlecha();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }

    void FuerzaFlecha()
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        rb.velocity = direction * throwSpeedMultiplier;
    }
}