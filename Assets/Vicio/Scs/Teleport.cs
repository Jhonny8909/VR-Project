using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.XR;

public class Teleport : MonoBehaviour
{
    public GameObject head;
    public GameObject player;
    public LineRenderer guide;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckInput();
    }

    void CheckInput()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices); // Este método debe ser reconocido
        
        foreach (var device in inputDevices)
        {
            // Verifica que sea el control izquierdo quien realiza la accion
            if ((device.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
            {
                bool triggerValue;

                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
                {
                    RaycastHit hit;
                    Vector3 ubi = new Vector3();

                    if (Physics.Raycast(head.transform.position, head.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {
                        guide.enabled = true;
                        guide.SetPosition(0, player.transform.position);
                        guide.SetPosition(1, hit.point);
                        
                        Debug.Log("Tp true");
                        
                        if (hit.transform.CompareTag("Tp"))
                        {
                            player.transform.position = new Vector3(ubi.x, ubi.y, ubi.z); 
                        }
                        
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
