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
    
    public float throwCooldown;

    bool readyToThrow;

    bool triggerPressed = false;

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

        tk.totalThrows--;
        
        readyToThrow = false;

        tk.knifeContact = false;

        Invoke(nameof(ResetThrow),throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

   void CheckInput()
   {
       var inputDevices = new List<InputDevice>();
       InputDevices.GetDevices(inputDevices); 

       foreach (var device in inputDevices)
       {
           
           if ((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
           {
               bool triggerValue;
               
                while (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue && tk.knifeContact && readyToThrow && tk.totalThrows > 0) {
                    triggerPressed = true;
                }
               /*if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue && tk.knifeContact && readyToThrow && tk.totalThrows > 0)
               {
                    triggerPressed = true;  
               }*/
               if(!triggerPressed && tk.instan == true)
                {
                    Throw();
                    Debug.Log("SI");
                }
           }
       }
    }
  }
   