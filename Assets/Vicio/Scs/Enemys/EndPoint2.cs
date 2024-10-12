using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint2 : MonoBehaviour
{
    private void OnDisable()
    {
        SceneManager.LoadScene("GrayBox2.0");
    }

}
