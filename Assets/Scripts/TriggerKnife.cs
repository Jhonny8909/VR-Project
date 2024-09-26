using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerKnife : MonoBehaviour
{
    public bool knifeContact;
    public GameObject knife;
    public Transform Parent;
    public float totalThrows;
    public bool instan;

    private void Awake()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KnifeSpace"))
        {
            knife = Instantiate(knife, Parent); 
            knife.GetComponent<Rigidbody>().isKinematic = true;
            knife.transform.localPosition = Vector3.zero;
            knifeContact= true;
            instan = true;
            Debug.Log("KnifeCollider");
        }
    }
}
