using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKnife : MonoBehaviour
{
    FeedBackDa�o fdb;
    private void OnColliderEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           StartCoroutine(fdb.damageflash());
           collision.gameObject.SetActive(false);
           Destroy(this);
        }
    }
}
