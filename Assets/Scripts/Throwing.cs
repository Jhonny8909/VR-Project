using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Throwing : MonoBehaviour
{
    public Transform attackPoint;
    public TriggerKnife triggerKnife;
    public float throwCooldown;
    public float throwSpeedMultiplier = 10f; // Multiplicador para ajustar la velocidad del lanzamiento
    public float minThrowMovementThreshold = 0.1f; // Umbral m�nimo de movimiento para lanzar

    private bool readyToThrow = true;
    private bool triggerPressed = false;
    private Vector3 lastPosition;

    private void Awake()
    {
        if (triggerKnife == null)
        {
            Debug.LogError("TriggerKnife reference is missing!");
        }
        lastPosition = attackPoint.position; // Inicializar la �ltima posici�n
    }

    private void Update()
    {
        CheckInput();
    }

    private void Throw()
    {
        if (triggerKnife.heldKnife == null) return;

        // Calcular la direcci�n del movimiento del brazo
        Vector3 currentPosition = attackPoint.position;
        Vector3 throwDirection = (currentPosition - lastPosition); // Direcci�n del movimiento
        lastPosition = currentPosition; // Actualizar la �ltima posici�n

        // Verificar si el movimiento es suficiente para lanzar
        if (throwDirection.magnitude > minThrowMovementThreshold)
        {
            Rigidbody rb = triggerKnife.heldKnife.GetComponent<Rigidbody>();
            if (rb != null)
            {
                triggerKnife.heldKnife.transform.parent = null;
                rb.isKinematic = false; // Aseg�rate de que el cuchillo no sea cinem�tico
                rb.velocity = throwDirection.normalized * throwSpeedMultiplier; // Establecer la velocidad directamente
            }

            triggerKnife.maxKnives--;
            readyToThrow = false;

            StartCoroutine(ThrowCooldown());
        }
        else
        {
            Debug.Log("No se realiz� un movimiento suficiente para lanzar.");
        }
    }

    private IEnumerator<WaitForSeconds> ThrowCooldown()
    {
        yield return new WaitForSeconds(throwCooldown);
        ResetThrow();
    }

    private void ResetThrow()
    {
        readyToThrow = true;

        if (triggerKnife.heldKnife != null)
        {
            triggerKnife.heldKnife.GetComponent<Collider>().isTrigger = false;
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
                    if (triggerValue && !triggerPressed && readyToThrow && triggerKnife.maxKnives > 0)
                    {
                        triggerPressed = true; // Gatillo presionado
                    }
                    else if (!triggerValue && triggerPressed) // Gatillo soltado
                    {
                        Throw(); // Llama a la funci�n Throw
                        Debug.Log("Knife thrown!");
                        triggerPressed = false; // Resetea el estado del gatillo
                    }
                }
            }
        }
    }
}