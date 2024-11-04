using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class BulletTime : MonoBehaviour
{
    public float BulletTimeCounter = 5f;
    private GameManager gameManager;
    private float checkingTime;
    public float BulletTimeQuantity = 3;
    

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckTime(); 
    }

    public static event Action<float> TimeSlow;

    public void CheckInput()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if ((device.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
            {
                if (device.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerValue) && BulletTimeQuantity > 0)
                {
                    TimeSlow?.Invoke(0.1f);
                    BulletTimeQuantity--;
                }
            }
        }
    }

    void CheckTime()
    {
        checkingTime = Time.deltaTime;

        if (checkingTime > BulletTimeCounter)
        {
            gameManager.GameTime = 1.0f;
        }
    }
}
