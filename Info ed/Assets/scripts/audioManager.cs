using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Source")]
    public AudioSource musicSource;

    [Header("SFX Source")]
    public AudioSource sfxSource;

    [Header("Gameplay Playlist")]
    public List<AudioClip> gameplayTracks = new List<AudioClip>();

    [Header("Menu/Tutorial Playlist")]
    public List<AudioClip> tutorialTracks = new List<AudioClip>();

    [Header("SFX Clips")]
    public AudioClip upgradeSFX;
    public AudioClip playerDeathSFX;
    public AudioClip reactorDeathSFX;
    public AudioClip shootSFX;

    [Header("Volumes")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private int currentTrackIndex = 0;
    private bool isGameplayMusic = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadVolumes();
        UpdateVolumes();

        // IMPORTANT: dacă scena NU se încarcă prin SceneManager,
        // OnSceneLoaded NU se apelează. Deci verificăm scena manual.
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "SampleScene")
            PlayGameplayMusic();
        else if (sceneName == "Tutorial" || sceneName == "MainMenu")
            PlayTutorialMusic();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
            PlayNextTrack();

        UpdateVolumes();
    }

    // ---------------- SCENE LOGIC ----------------

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "MainMenu")
        {
            PlayTutorialMusic();
        }
        else if (sceneName == "Tutorial")
        {
            PlayTutorialMusic();
        }
        else if (sceneName == "SampleScene")
        {
            PlayGameplayMusic();
        }
    }

    // ---------------- MUSIC CONTROL ----------------

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayGameplayMusic()
    {
        if (isGameplayMusic) return;

        StopMusic();
        isGameplayMusic = true;
        currentTrackIndex = 0;
        PlayTrackFromList(gameplayTracks);
    }

    public void PlayTutorialMusic()
    {
        if (!isGameplayMusic && musicSource.isPlaying) return;

        isGameplayMusic = false;
        currentTrackIndex = 0;
        PlayTrackFromList(tutorialTracks);
    }

    private void PlayTrackFromList(List<AudioClip> list)
    {
        if (list.Count == 0)
            return;

        musicSource.clip = list[currentTrackIndex];
        musicSource.loop = false;
        musicSource.Play();
    }

    private void PlayNextTrack()
    {
        List<AudioClip> list = isGameplayMusic ? gameplayTracks : tutorialTracks;

        if (list.Count == 0)
            return;

        currentTrackIndex++;
        if (currentTrackIndex >= list.Count)
            currentTrackIndex = 0;

        PlayTrackFromList(list);
    }

    // ---------------- SFX CONTROL ----------------

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, masterVolume * sfxVolume);
    }

    // ---------------- VOLUME CONTROL ----------------

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        UpdateVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        UpdateVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        UpdateVolumes();
    }

    public void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void UpdateVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;
        sfxSource.volume = masterVolume * sfxVolume;
    }
}
