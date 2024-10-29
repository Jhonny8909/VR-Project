using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.UI;

public class VRInvisibility : MonoBehaviour
{
    public float invisibilityDuration = 5f;
    public float cooldownDuration = 10f;
    public bool IsInvisible = false;
    public Image InvisibilitySlider;

    public InputActionProperty vrButtonAction;
    public Material invisibleMaterial;
    public Material originalMaterial;
    private Renderer playerRenderer;
    public MeshRenderer controlDerecho;
    public MeshRenderer controlIzq;

    public float elapsedTimeInvisibility;
    public float elapsedTimeInvisibilityDuration = 1f;
    public float elapsedTimeInvisibilitySlider;

    void Update()
    {
        elapsedTimeInvisibility += Time.deltaTime;
        elapsedTimeInvisibilityDuration -= Time.deltaTime / 4;
        elapsedTimeInvisibilitySlider += Time.deltaTime / 14;
        CheckGripButton();
    }

    void ActivateInvisibility()
    {
        Debug.Log("lapsed " + elapsedTimeInvisibility);
        Debug.Log("cooldown " + cooldownDuration);
        //if (IsInvisible) return;

        if (elapsedTimeInvisibility > cooldownDuration && IsInvisible == false)
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

        if (elapsedTimeInvisibilityDuration < invisibilityDuration)
        {
            IsInvisible = false;
            Debug.Log("El jugador ya no es invisible.");

            Debug.Log("invnt");
            controlDerecho.material = originalMaterial;
            controlIzq.material = originalMaterial;
            elapsedTimeInvisibility = 0;
            elapsedTimeInvisibilitySlider = 0;
        }
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

    void UpdateInvisibilityCooldown()
    {
        if (InvisibilitySlider != null)
        {
            if(IsInvisible == false)
            {
                InvisibilitySlider.fillAmount = elapsedTimeInvisibilitySlider;
            }
            if (IsInvisible == true)
            {
                InvisibilitySlider.fillAmount = invisibilityDuration;
            }
        }
    }
}
