using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageKnife : MonoBehaviour
{
    FeedBackDano fdb;
    Boss FinalBoss;

    private void Start()
    {
        FinalBoss = FindAnyObjectByType<Boss>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            FinalBoss.life--;
            Destroy(this.gameObject);

            if (FinalBoss.life <= 0)
            {
                other.gameObject.SetActive(false);
                SceneManager.LoadScene(FinalBoss.nextLevel);
            }
        }
    }
}
