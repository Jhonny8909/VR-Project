using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public string NameScene;
    private void OnTriggerEnter(Collider other)
    {
        other.CompareTag("Knife");
        SceneManager.LoadScene(NameScene, LoadSceneMode.Single);
    }
}
