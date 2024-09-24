using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackDaño : MonoBehaviour
{
    public Color Originalcolor;
    public Color Damagecolor;
    public float damageflashduration;

    private void Start()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Weapon"))
        {
            StartCoroutine(damageflash());

        }
    }

    IEnumerator damageflash()
    {
        GetComponent<MeshRenderer>().material.color=Damagecolor;
        yield return new WaitForSeconds(damageflashduration);
        GetComponent<MeshRenderer>().material.color = Originalcolor;
    }

}
