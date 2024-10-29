using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public float GameTime = 1.0f;
    //public float GamingTime;
    public bool bulletTime;
    public static GameManager Instance {
        get
        {
            if (instance == null)
            {
                GameObject gameManager = new GameObject();
                instance = gameManager.AddComponent<GameManager>();
                gameManager.name = "Game Manager Singleton";

            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        BulletTime.TimeSlow += ChangeTime; 
    }

    public void ChangeTime(float tiempo)
    {
        GameTime = tiempo;
    }

}
