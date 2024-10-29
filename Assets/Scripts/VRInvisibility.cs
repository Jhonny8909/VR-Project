using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR;
using System.Collections.Generic;

public class VRInvisibility : MonoBehaviour
{
    public float invisibilityDuration = 5f;
    public float cooldownDuration = 10f;
    public bool IsInvisible;
    public bool isInCooldown = false;

    public InputActionProperty vrButtonAction;
    public Material invisibleMaterial;
    public Material originalMaterial;
    private Renderer playerRenderer;
    public MeshRenderer controlDerecho;
    public MeshRenderer controlIzq;

    public float elapsedTimeInvisibility;
    public float elapsedTimeInvisibilitySlider;

    void Update()
    {
        elapsedTimeInvisibility += Time.deltaTime;
        elapsedTimeInvisibilitySlider += Time.deltaTime / 14;
        CheckGripButton();
    }

    void ActivateInvisibility()
    {
        Debug.Log("lapsed " + elapsedTimeInvisibility);
        Debug.Log("cooldown " + cooldownDuration);
        //if (IsInvisible) return;

        if (elapsedTimeInvisibility > cooldownDuration)
        {
            StartCoroutine(InvisibilityRoutine());
        }   
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

    }

     
    void CheckGripButton()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevices(inputDevices);
        
        foreach (var device in inputDevices)
        {
            if ((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
            {
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue))
                {
                    if (triggerValue)
                    {
                        ActivateInvisibility();
                        Debug.Log("Se activa la invisibilidad");
                    }
                }
            }
        }
    }
}
