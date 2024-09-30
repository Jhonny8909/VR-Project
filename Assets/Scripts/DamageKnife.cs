using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKnife : MonoBehaviour
{
    private void OnColliderEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
