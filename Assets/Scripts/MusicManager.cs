using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] AudioClip mainMenuTheme, gameTheme;
    [SerializeField] float mainMenuVolume = 0.166f, gameVolume = 0.166f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayTrack(AudioClip track, float volume)
    {
        audioSource.volume = volume;
        if (audioSource.clip == track && audioSource.isPlaying) return;
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = track;
        audioSource.Play();
    }

    public void PlayMainMenuTheme()
    {
        PlayTrack(mainMenuTheme, mainMenuVolume);
    }

    public void PlayGameTheme()
    {
        PlayTrack(gameTheme, gameVolume);
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
