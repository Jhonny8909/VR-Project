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
    [HideInInspector]
    public CircleCollider2D circleCollider;

    private void Awake()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KnifeSpace"))
        {
            if(knife != null)
            {
                Debug.Log("Knife ya spawneado");
            }
            else
            {
                knife = Instantiate(knife, Parent);
                knife.GetComponent<Rigidbody>().isKinematic = true;
                knife.transform.localPosition = Vector3.zero;
                knife.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                knifeContact = true;
                instan = true;
                Debug.Log("KnifeCollider");
            }
        }
    }
}
