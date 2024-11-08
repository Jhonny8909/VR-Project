using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{
    public GameObject PanelVictoria;
    GameManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Final"))
        {
            PanelVictoria.SetActive(true);
            manager.GameTime = 0;
        }
    }
}
