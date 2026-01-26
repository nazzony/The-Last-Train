using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip clickSound;
    public AudioClip coinSound;
    public AudioClip keySound;
    public AudioClip doorSound;
    public AudioClip trashSound;
    public AudioClip vendingSound;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void playSFX(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
}
