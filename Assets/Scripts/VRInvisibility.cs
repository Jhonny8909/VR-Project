using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit; 
using System.Collections;
using UnityEngine.XR;
using System.Collections.Generic;

public class VRInvisibility : MonoBehaviour
{
    public float invisibilityDuration = 5f; 
    public float cooldownDuration = 15f;    
    public bool IsInvisible { get; private set; }  
    private bool isInCooldown = false; 

    public InputActionProperty vrButtonAction;  
    public Material invisibleMaterial; 
    public Material originalMaterial; 
    private Renderer playerRenderer;   

    private XRController controllerDevice;

    void Start()
    {

        playerRenderer = GetComponent<Renderer>();

        if (playerRenderer != null)
        {
            originalMaterial = playerRenderer.material; 
        }

        controllerDevice = GetComponent<XRController>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && !isInCooldown)
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
        }
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

        if (playerRenderer != null)
        {
            playerRenderer.material = invisibleMaterial;
        }

        yield return new WaitForSeconds(invisibilityDuration);

        IsInvisible = false;
        Debug.Log("El jugador ya no es invisible.");

        if (playerRenderer != null)
        {
            playerRenderer.material = originalMaterial;
        }

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
}
