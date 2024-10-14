using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint3 : MonoBehaviour
{ 
    private void OnDisable()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
