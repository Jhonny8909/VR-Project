using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Knife Sounds")]
    public AudioClip knifeDraw;
    public AudioClip knifeAim;
    public AudioClip knifeThrow;
    public AudioClip knifeHit;

    [Header("Player Sounds")]
    public AudioClip playerRespawn;
    public AudioClip playerDeath;
    public AudioClip playerWalk;

    [Header("Teleport Sounds")]
    public AudioClip teleportAim;
    public AudioClip teleportLaunch;
    public AudioClip teleportReload;
    public AudioClip teleportHit;

    [Header("Invisibility Sounds")]
    public AudioClip invisibilityOn;
    public AudioClip invisibilityActive;
    public AudioClip invisibilityOff;
    public AudioClip invisibilityReload;

    [Header("Enemy Sounds")]
    public AudioClip enemyWalk;
    public AudioClip enemyAttack;
    public AudioClip enemyHitPlayer;
    public AudioClip enemyDetectPlayer;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    private AudioSource audioSource;
    private AudioSource bgMusicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        bgMusicSource = gameObject.AddComponent<AudioSource>();

        bgMusicSource.clip = backgroundMusic;
        bgMusicSource.loop = true;
        bgMusicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
