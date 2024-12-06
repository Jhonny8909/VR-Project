using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKnife : MonoBehaviour
{
    FeedBackDaño fdb;
    Boss FinalBoss;

    private void Start()
    {
        FinalBoss = FindAnyObjectByType<Boss>();
    }
    private void OnColliderEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           StartCoroutine(fdb.damageflash());
           collision.gameObject.SetActive(false);
           Destroy(this.gameObject);
        }

        if (collision.CompareTag("Boss"))
        {
            StartCoroutine(fdb.damageflash());
            FinalBoss.life--;
            Destroy(this);

            if (FinalBoss.life <= 0)
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
