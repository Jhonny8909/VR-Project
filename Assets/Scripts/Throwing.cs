using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Throwing : MonoBehaviour
{
    public Transform attackPoint;
    public TriggerKnife triggerKnife;
    public float throwCooldown;
    public float throwSpeedMultiplier = 10f; // Multiplicador para ajustar la velocidad del lanzamiento
    public float minThrowMovementThreshold = 0.1f; // Umbral mínimo de movimiento para lanzar

    private bool readyToThrow = true;
    private bool triggerPressed = false;
    private Vector3 lastPosition;

    private void Awake()
    {
        if (triggerKnife == null)
        {
            Debug.LogError("TriggerKnife reference is missing!");
        }
        lastPosition = attackPoint.position; // Inicializar la última posición
    }

    private void Update()
    {
        CheckInput();
    }

    private void Throw()
    {
        if (triggerKnife.heldKnife == null) return;

        // Calcular la dirección del movimiento del brazo
        Vector3 currentPosition = attackPoint.position;
        Vector3 throwDirection = (currentPosition - lastPosition); // Dirección del movimiento
        lastPosition = currentPosition; // Actualizar la última posición

        // Verificar si el movimiento es suficiente para lanzar
        if (throwDirection.magnitude > minThrowMovementThreshold)
        {
            Rigidbody rb = triggerKnife.heldKnife.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Asegúrate de que el cuchillo no sea cinemático
                rb.velocity = throwDirection.normalized * throwSpeedMultiplier; // Establecer la velocidad directamente
            }

            triggerKnife.maxKnives--;
            readyToThrow = false;

            StartCoroutine(ThrowCooldown());
        }
        else
        {
            Debug.Log("No se realizó un movimiento suficiente para lanzar.");
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
                        Throw(); // Llama a la función Throw
                        Debug.Log("Knife thrown!");
                        triggerPressed = false; // Resetea el estado del gatillo
                    }
                }
            }
        }
    }
}