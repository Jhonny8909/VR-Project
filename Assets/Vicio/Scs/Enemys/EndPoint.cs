using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    private void OnDisable()
    {
        SceneManager.LoadScene("nivel2");
    }
}
