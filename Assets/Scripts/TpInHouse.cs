using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TpInHouse : MonoBehaviour
{
    public GameObject player;
    public GameObject head;
    public LineRenderer guide;
    void FixedUpdate()
    {
        CheckInput(); // Llamar en FixedUpdate para verificar continuamente
    }

    void CheckInput()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices); // Este método debe ser reconocido

        foreach (var device in inputDevices)
        {
            // Verificar si el dispositivo es el controlador izquierdo
            if ((device.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
            {
                bool triggerValue;
                bool releaseChecker; // --> va a checar si durante el frame anterior la booleana triggerValue era true, de ser lo, va a activar el tp
                //si durante el frame anterior triggerValue era falso, no hara nada.
                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
                {
                    RaycastHit hit;
                    
                    if (Physics.Raycast(head.transform.position, head.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {
                        guide.enabled = true;
                        guide.SetPosition(0, player.transform.position);
                        guide.SetPosition(1, hit.point);
                        
                        Debug.Log("Tp true");

                        Vector3 ubi = new Vector3();
                        if (hit.transform.CompareTag("Tp"))
                        {
                            ubi = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
                            player.transform.position = new Vector3(ubi.x, ubi.y, ubi.z);
                        }
                        
                    }
                    else
                    {
                        guide.enabled = true;
                        guide.SetPosition(0, player.transform.position);
                        guide.SetPosition(1, head.transform.forward*100);

                    }
                }
                else
                {
                    guide.enabled = false;
                }
            }
        }
    }
    
}