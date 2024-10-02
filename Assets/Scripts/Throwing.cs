using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Throwing : MonoBehaviour
{
    public Transform attackPoint;
    public TriggerKnife triggerKnife; // Renamed for clarity
    public float throwCooldown;

    private bool readyToThrow = true;
    private bool triggerPressed = false;

    private void Awake()
    {
        // Ensure we have a valid triggerKnife reference
        if (triggerKnife == null)
        {
            Debug.LogError("TriggerKnife reference is missing!");
        }
    }

    private void Update()
    {
        CheckInput();
    }

    private void Throw()
    {
        if (triggerKnife.heldKnife == null) return;

        // Detach knife from the parent
        triggerKnife.heldKnife.transform.parent = null;

        Rigidbody rb = triggerKnife.heldKnife.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Make sure the knife is not kinematic
        }

        triggerKnife.totalThrows--;
        readyToThrow = false;

        StartCoroutine(ThrowCooldown());
    }

    private IEnumerator ThrowCooldown()
    {
        yield return new WaitForSeconds(throwCooldown);
        ResetThrow();
    }

    private void ResetThrow()
    {
        readyToThrow = true;
        if (triggerKnife.heldKnife != null)
        {
            triggerKnife.heldKnife.GetComponent<Collider>().isTrigger = false; // Make collider non-trigger
        }
    }

    private void CheckInput()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if ((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
            {
                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue))
                {
                    if (triggerValue && !triggerPressed && readyToThrow && triggerKnife.totalThrows > 0)
                    {
                        // Gatillo presionado por primera vez
                        triggerPressed = true;
                    }
                    else if (!triggerValue && triggerPressed) // Gatillo soltado
                    {
                        Throw();
                        Debug.Log("Knife thrown!");
                        triggerPressed = false; // Resetea el estado del gatillo
                    }
                }
            }
        }
    }
}
