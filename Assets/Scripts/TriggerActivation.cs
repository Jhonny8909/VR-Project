using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class TriggerActivation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        TriggerPressed();
    }

    void TriggerPressed()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if ((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
            {
                bool triggerValue;

                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
                {
                    this.GetComponent<Collider>().enabled = true;

                }
                else
                {
                    this.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
