using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit; 
using System.Collections;
using UnityEngine.XR;
using System.Collections.Generic;

public class VRInvisibility : MonoBehaviour
{
    public float invisibilityDuration = 5f;
    public float cooldownDuration = 2f;
    public bool IsInvisible;
    private bool isInCooldown = false;

    public InputActionProperty vrButtonAction;
    public Material invisibleMaterial;
    public Material originalMaterial;
    private Renderer playerRenderer;
    public MeshRenderer controlDerecho;
    public MeshRenderer controlIzq;

private XRController controllerDevice;

    void Start()
    {

        //controllerDevice = GetComponent<XRController>();
    }

    void Update()
    {
        /*if (Keyboard.current.eKey.wasPressedThisFrame && !isInCooldown)
        {
            ActivateInvisibility();
        }


        if (controllerDevice != null)
        {
            if (controllerDevice.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out bool triggerButtonPressed) && triggerButtonPressed && !isInCooldown)
            {
                ActivateInvisibility();
            }
        }

        if (vrButtonAction.action != null && vrButtonAction.action.WasPressedThisFrame() && !isInCooldown)
        {
            ActivateInvisibility();
        }*/

        CheckGripButton();
    }

    void ActivateInvisibility()
    {
        if (IsInvisible || isInCooldown) return;

        StartCoroutine(InvisibilityRoutine());
    }

    private IEnumerator InvisibilityRoutine()
    {
        IsInvisible = true;
        Debug.Log("El jugador es invisible.");
        
            
            Debug.Log(("Inv"));
            controlDerecho.material = invisibleMaterial;
            controlIzq.material = invisibleMaterial;

        yield return new WaitForSeconds(invisibilityDuration);

        IsInvisible = false;
        Debug.Log("El jugador ya no es invisible.");

             Debug.Log("invnt");
            controlDerecho.material = originalMaterial;
            controlIzq.material = originalMaterial;
        

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isInCooldown = true;
        Debug.Log("Cooldown iniciado.");
        yield return new WaitForSeconds(cooldownDuration);
        isInCooldown = false;
        Debug.Log("Cooldown terminado.");
    }

    void CheckGripButton()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevices(inputDevices);
        
        foreach (var device in inputDevices)
        {
            if ((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
            {
                bool triggerValue;

                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out triggerValue) && triggerValue && !isInCooldown)
                {
                    ActivateInvisibility();
                }
            }
        }
    }
}
