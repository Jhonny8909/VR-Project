using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using System.Runtime.CompilerServices;

public class Throwing : MonoBehaviour
{
    
    public Transform attackPoint;
    
    public TriggerKnife tk;
    
    public int totalThrows;
    public float throwCooldown;

    //public float throwForce;
    //public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        tk.GetComponent<TriggerKnife>();
    }
    private void Awake()
    {
        readyToThrow = true;
        tk.knifeContact = false;
    }

    private void Update()
    {
        CheckInput();
        //MouseInput();
    }
    private void Throw()
    {
        tk.knife.transform.parent = null;

        Rigidbody rb = tk.knife.GetComponent<Rigidbody>();

        if (rb != null)
        {
            tk.knife.GetComponent<Rigidbody>().isKinematic = false;
        }        
        
        //GameObject projectile = Instantiate(knife, attackPoint.position, attackPoint.rotation);
        
        /*Vector3 forceDirection = attackPoint.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        rb.AddForce(forceToAdd, ForceMode.Impulse); */

        totalThrows--;
        
        readyToThrow = false;


        tk.knifeContact = false;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

   /* void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && readyToThrow && totalThrows > 0 && knifeContact)
        {
            Throw();
        }
    }*/
   void CheckInput()
   {
       var inputDevices = new List<InputDevice>();
       InputDevices.GetDevices(inputDevices); 

       foreach (var device in inputDevices)
       {
           
           if ((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
           {
               bool triggerValue;
                
               if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue && tk.knifeContact && readyToThrow && totalThrows > 0)
               {
                   Throw();
                   Debug.Log("SI");
               }
           }
       }
    }
   }
   