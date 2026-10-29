using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Sound Clips")]
    public AudioClip playerHitSound;
    public AudioClip poacherTelegraphSound;
    public AudioClip enemySpawnSound;

    public AudioClip punchSound;
    public AudioClip poundSound;

    public AudioClip gorrillamovement;
    public AudioClip screamgorrilla;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps audio playing across scene reloads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this function from any script to play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}