using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using System.Runtime.CompilerServices;

public class Throwing : MonoBehaviour
{
    
    public Transform cam;
    public Transform attackPoint;
    public GameObject knife;
    public Transform Parent;
    bool knifeContact;
    
    public int totalThrows;
    public float throwCooldown;

    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
    }
    private void Awake()
    {
        knifeContact = false;
    }

    private void Update()
    {
        //CheckInput();
        MouseInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KnifeSpace"))
        {
            knife = Instantiate(knife, Parent); 
            knife.GetComponent<Rigidbody>().isKinematic = true;
            knifeContact= true;
            Debug.Log("KnifeCollider");
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        knife.transform.parent = null;

        knife.GetComponent<Rigidbody>().isKinematic=false;

        
        GameObject projectile = Instantiate(knife, cam.position, attackPoint.rotation);

        
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        
        Vector3 forceDirection = attackPoint.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        knifeContact = false;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && readyToThrow && totalThrows > 0 && knifeContact)
        {
            Throw();
        }
    }
    /*
    void CheckInput()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            bool triggerValue;
            if(device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue && readyToThrow && totalThrows > 0)
            {
                Throw ();
                Debug.Log("TriggerButton is pressed");
            }
        }
    }*/
}