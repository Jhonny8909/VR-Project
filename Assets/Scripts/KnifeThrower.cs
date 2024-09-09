using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KnifeThrower : MonoBehaviour
{
    public GameObject knifePrefab;
    public Transform handPosition;
    public float throwforce = 10f;
    // Start is called before the first frame update
    void ThrowKnife()
    {
        GameObject knife = Instantiate(knifePrefab, handPosition.position, handPosition.rotation);
        Rigidbody rb = knife.GetComponent<Rigidbody>();
        rb.AddForce(handPosition.forward * throwforce, ForceMode.Impulse);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
