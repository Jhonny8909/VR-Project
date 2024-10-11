using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Tp2 : MonoBehaviour
{
     public GameObject head;
    public GameObject player;
    public LineRenderer guide;

    private bool _previousTriggerValue = false;
    private bool _triggerReleased = false;

    private Material purple;

    void Start()
    {
        purple = GetComponent<LineRenderer>().material;
    }
    
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
                guide.enabled = false;

                bool currentTriggerValue; // Asigna el estado actual del gatillo

                // Activa el raycast cuando se presiona el gatillo
                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out currentTriggerValue))
                {
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(player.transform.position + Vector3.up / 3,
                        head.transform.TransformDirection(Vector3.forward), out hit, 30); // guarda la inforacion de raycast
                    
                    guide.enabled = true;
                    guide.SetPosition(0, player.transform.position + Vector3.up/3);
                    guide.SetPosition(1, hasHit ? hit.point : head.transform.position + head.transform.TransformDirection(Vector3.forward) * 30);
                    
                    if(guide.transform.CompareTag("Tp"))
                    {
                        purple.color = Color.blue;
                    }
                    else
                    {
                        purple.color = Color.red;
                    }
                    
                    Vector3 ubi = new Vector3();

                    
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
                                
                                ubi = new Vector3(hit.transform.position.x, hit.transform.position.y,
                                    hit.transform.position.z);
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
                    
                }
            }
        }
    }
}
