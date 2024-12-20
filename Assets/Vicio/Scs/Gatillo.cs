using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Gatillo : MonoBehaviour
{
    public GameObject head;
    public GameObject player;
    
    private bool previousTriggerValue;
    private bool triggerReleased;
    private RaycastHit raycastHitInfo;
    private bool hasHit;

    public Image mira1;
    public Image mira2;

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckInput();
    }

    void CheckInput()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices); // Asigna los controles a la variable
        
        foreach (var device in inputDevices)
        {
            // Verifica que sea el control izquierdo quien realiza la accion
            if ((device.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
            {
                bool currentTriggerValue; // Asigna el estado actual del gatillo

                // Activa el raycast cuando se presiona el gatillo
                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out currentTriggerValue))
                {
                    Ray ray = new Ray(player.transform.position + Vector3.up / 2,
                        head.transform.TransformDirection(Vector3.forward));
                    float maxDistance = 20f;
                    hasHit = Physics.Raycast(ray, out raycastHitInfo, maxDistance);
                    /*
                    RaycastHit hit; 
                    hasHit = Physics.Raycast(player.transform.position + Vector3.up / 2,
                        head.transform.TransformDirection(Vector3.forward), out hit, 100); // guarda la inforacion de raycast
                    */ 
                    if (hasHit)
                    {
                        Debug.Log("Rayito casto");
                        
                        if (hasHit)
                        {
                            // La reticula central que indica si es posible tepear cambia depediendo de donde apunta
                            if (raycastHitInfo.transform.CompareTag("Tp"))
                            {
                                this.mira1.GetComponent<Image>().enabled = true;
                                this.mira2.GetComponent<Image>().enabled = false;
                            }
                            else if (raycastHitInfo.transform.CompareTag("Ground"))
                            {
                                this.mira1.GetComponent<Image>().enabled = true;
                                this.mira2.GetComponent<Image>().enabled = false;
                            }
                            else
                            {
                                this.mira1.GetComponent<Image>().enabled = false;
                                this.mira2.GetComponent<Image>().enabled = true;
                            } 
                        }
                        
                        // Verifica si el gatillo fue soltado
                        if (previousTriggerValue && !currentTriggerValue)
                        {
                            Vector3 ubi;
                            Vector3 groundUbi;
                            
                            triggerReleased = true;
                            Debug.Log("Gatillo soltado");
                            
                            // Si se suelta el gatillo sobre un objeto con el Tag Tp se teletransporta
                            if(triggerReleased == true && raycastHitInfo.transform.CompareTag("Tp"))
                            {
                                Debug.Log("Se solto sobre algo tepeable");
                                
                                ubi = new Vector3(raycastHitInfo.transform.position.x, raycastHitInfo.transform.position.y,
                                    raycastHitInfo.transform.position.z);
                                player.transform.position = new Vector3(ubi.x, ubi.y + 3f, ubi.z);
                            }
                            else if (triggerReleased == true && raycastHitInfo.transform.CompareTag("Ground"))
                            {
                                groundUbi = new Vector3(raycastHitInfo.point.x, raycastHitInfo.point.y,
                                    raycastHitInfo.point.z);
                                player.transform.position = new Vector3(groundUbi.x, groundUbi.y, groundUbi.z);
                            }
                            else // Si el objeto no tiene el Tag, no hace nada
                            {
                                Debug.Log("Imposible tepear");
                            }
                            
                        }
                        else
                        {
                            triggerReleased = false;
                        }
                        
                        previousTriggerValue = currentTriggerValue; // Resete el estado del gatillo

                    }

                    if (device.TryGetFeatureValue(CommonUsages.primaryButton, out currentTriggerValue))
                    {
                        
                    }
                    
                }
            }
        }
    }
}
