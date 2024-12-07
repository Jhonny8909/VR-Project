using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackDano : MonoBehaviour
{
    public Color Originalcolor;
    public Color Damagecolor;
    public float damageflashduration;

    private void Start()
    {

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Knife"))
        {
            StartCoroutine(damageflash());

        }
    }*/

    public IEnumerator damageflash()
    {
        GetComponent<MeshRenderer>().material.color=Damagecolor;
        yield return new WaitForSeconds(damageflashduration);
        GetComponent<MeshRenderer>().material.color = Originalcolor;
    }

}
