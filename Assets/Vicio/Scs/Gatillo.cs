using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Gatillo : MonoBehaviour
{
    public GameObject head;
    public GameObject player;

    private bool _previousTriggerValue = false;
    private bool _triggerReleased = false;

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
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(player.transform.position + Vector3.up / 2,
                        head.transform.TransformDirection(Vector3.forward), out hit, 40); // guarda la inforacion de raycast
                    
                    Vector3 ubi = new Vector3();
                    if (hasHit)
                    {
                        if (hit.transform.CompareTag("Tp"))
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
                    
                    

                    if (hasHit)
                    {

                        Debug.Log("Rayito casto");
                        
                        // Verifica si el gatillo fue soltado
                        if (_previousTriggerValue && !currentTriggerValue)
                        {
                            _triggerReleased = true;
                            Debug.Log("Gatillo soltado");
                            
                            // Si se suelta el gatillo sobre un objeto con el Tag Tp se teletransporta
                            if(_triggerReleased == true && hit.transform.CompareTag("Tp"))
                            {
                                Debug.Log("Se solto sobre algo tepeable");
                                
                                ubi = new Vector3(hit.point.x, hit.point.y,
                                    hit.point.z);
                                player.transform.position = new Vector3(ubi.x, ubi.y, ubi.z);
                            }
                            else // Si el objeto no tiene el Tag, no hace nada
                            {
                                Debug.Log("Imposible tepear");
                            }
                        }
                        
                        else
                        {
                            _triggerReleased = false;
                        }
                        
                        _previousTriggerValue = currentTriggerValue; // Resete el estado del gatillo

                    }

                    if (device.TryGetFeatureValue(CommonUsages.primaryButton, out currentTriggerValue))
                    {
                        
                    }
                    
                }
            }
        }
    }
}
