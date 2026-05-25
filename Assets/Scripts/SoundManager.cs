using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("AudioSources")]
    int numOfAudioSources = 5;
    [SerializeField] GameObject soundPrefab;

    [Header("Normal Sounds")]
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip throwAttack;
    [SerializeField] AudioClip throwRecover;
    [SerializeField] AudioClip boingEnemy;
    [SerializeField] AudioClip damageToPlayer;
    [SerializeField] AudioClip damageToEnemy;
    [SerializeField] AudioClip damageToBox;
    [SerializeField] AudioClip starBreaking;
    [SerializeField] float dashVolume = 0.166f;
    [SerializeField] float attackVolume = 0.166f;
    [SerializeField] float jumpVolume = 0.166f;
    [SerializeField] float throwAttackVolume = 0.166f;
    [SerializeField] float throwRecoverVolume = 0.166f;
    [SerializeField] float boingEnemyVolume = 0.166f;
    [SerializeField] float damageToPlayerVolume = 0.166f;
    [SerializeField] float damageToEnemyVolume = 0.166f;
    [SerializeField] float damageToBoxVolume = 0.166f;
    [SerializeField] float starBreakingVolume = 0.166f;

    [Header("Loop sounds")]
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource tirolina;
    
    List<AudioSource> soundSources;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


        soundSources = new List<AudioSource>();
        for (int i = 0; i < numOfAudioSources; i++)
        {
            GameObject soundPf = Instantiate(soundPrefab, transform);
            AudioSource audioS = soundPf.GetComponent<AudioSource>();
            soundSources.Add(audioS);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        walk.Play();    
        tirolina.Play();

        walk.Stop();
        tirolina.Stop();
    }

    int nextSoundIndex = 0;
    private void PlaySound(AudioClip clip, float volume)
    {
        AudioSource source = soundSources[nextSoundIndex];
        source.Stop();
        source.volume = volume;
        source.clip = clip;
        source.Play();
        ++nextSoundIndex;
        if (nextSoundIndex >= soundSources.Count) nextSoundIndex = 0;
    }

    public void PlayDash()
    {
        PlaySound(dash, dashVolume);
    }

    public void PlayWalk(bool play)
    {
        if (play)
        {
            if (!walk.isPlaying) walk.Play();
        }
        else walk.Stop();
    }

    public void PlayAttack()
    {
        PlaySound(attack, attackVolume);
    }

    public void PlayJump()
    {
        PlaySound(jump, jumpVolume);
    }

    public void PlayThrowAttack()
    {
        PlaySound(throwAttack, throwAttackVolume);
    }

    public void PlayThrowRecover()
    {
        PlaySound(throwRecover, throwRecoverVolume);
    }

    public void PlayBoingEnemy()
    {
        PlaySound(boingEnemy, boingEnemyVolume);
    }

    public void PlayDamageToPlayer()
    {
        PlaySound(damageToPlayer, damageToPlayerVolume);
    }

    public void PlayDamageToEnemy()
    {
        PlaySound(damageToEnemy, damageToEnemyVolume);
    }

    public void PlayDamageToBox()
    {
        PlaySound(damageToBox, damageToBoxVolume);
    }

    public void PlayTirolina(bool play)
    {
        if (play)
        {
            if (!tirolina.isPlaying) tirolina.Play();
        }
        else tirolina.Stop();
    }

    public void PlayStarBreaking()
    {
        PlaySound(starBreaking, starBreakingVolume);
    }
}
