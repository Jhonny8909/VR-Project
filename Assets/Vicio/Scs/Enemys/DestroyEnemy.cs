using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
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
    }
}
