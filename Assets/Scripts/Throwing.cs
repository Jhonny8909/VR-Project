using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Throwing : MonoBehaviour
{
    public Transform attackPoint;
    public TriggerKnife triggerKnife;
    public float throwCooldown;
    public float throwSpeedMultiplier = 10f; // Multiplicador para ajustar la velocidad del lanzamiento
    public float minThrowMovementThreshold = 0.1f; // Umbral minimo de movimiento para lanzar

    private bool readyToThrow = true;
    private bool triggerPressed = false;
    private Vector3 lastPosition, currentPosition, throwDirection;

    private GameManager gameManager;
    

    private void Awake()
    {
        if (triggerKnife == null)
        {
            Debug.LogError("TriggerKnife reference is missing!");
        }
        //lastPosition = attackPoint.position; // Inicializar la ultima posicion
    }

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        CheckInput();
    }

    private void Throw(Vector3 trueDirection)
    {
        if (triggerKnife.heldKnife == null) return; // Verificar si el cuchillo esta en mano

        //Calcular la direccion del movimiento del brazo
        //trueDirection = attackPoint.transform.forward;


        trueDirection = Camera.main.transform.forward;
        
        if (trueDirection.magnitude > minThrowMovementThreshold)
        {
            Rigidbody rb = triggerKnife.heldKnife.GetComponent<Rigidbody>();
            if (rb != null)
            {
                triggerKnife.heldKnife.transform.parent = null;
                rb.isKinematic = false;
                rb.velocity = trueDirection * throwSpeedMultiplier * gameManager.GameTime;
            }

            triggerKnife.KnifeThrown(); // Llama a KnifeThrown despues de lanzar
            Debug.Log("Cuchillo lanzado.");
        }
        else
        {
            Debug.Log("No se realizo un movimiento suficiente para lanzar.");
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
            triggerKnife.KnifeThrown();
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
                if (device.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerValue))
                {
                    if (triggerValue && !triggerPressed && readyToThrow && triggerKnife.maxKnives > 0)
                    {
                        triggerPressed = true; // Gatillo presionado
                        lastPosition = attackPoint.transform.position;
                        //primer punto
                    }
                    else if (!triggerValue && triggerPressed) // Gatillo soltado
                    {
                        //segundo punto
                        currentPosition = attackPoint.transform.position;
                        throwDirection = (currentPosition - lastPosition).normalized;
                        Throw(throwDirection); // Llama a la funcion Throw
                        Debug.Log("Knife thrown!");
                        triggerPressed = false; // Resetea el estado del gatillo
                    }
                }
            }
        }
    }
}