using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SH
public class GlobalMusicManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [SerializeField] private List<SceneMusic> sceneMusicList;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 2f;

    public static GlobalMusicManager Instance { get; private set; }

    private Coroutine fadeCoroutine;
    private AudioClip intendedClip = null;
    private string intendedScene = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (Settings.Instance != null)
            musicSource.volume = Settings.Instance.MusicVolume;
    }    

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneMusic targetMusic = sceneMusicList.Find(m => m.sceneName == scene.name);

        AudioClip newClip = targetMusic != null ? targetMusic.musicClip : null;
        AudioClip currentClip = musicSource.clip;

        intendedClip = newClip;
        intendedScene = scene.name;

        // If no music for this scene, fade out and stop
        if (newClip == null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeOutAndStopSafe(scene.name));
            return;
        }

        // If same clip, just ensure it's playing and at correct volume
        if (currentClip == newClip)
        {
            if (!musicSource.isPlaying)
                musicSource.Play();
            if (Settings.Instance != null)
                musicSource.volume = Settings.Instance.MusicVolume;
            return;
        }

        // Otherwise, fade out current and fade in new
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutInSafe(newClip, scene.name));
    }

    private IEnumerator FadeOutInSafe(AudioClip newClip, string sceneName)
    {
        float startVolume = musicSource.volume;
        float time = 0f;

        // Fade out
        while (time < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
            // If scene changed, abort
            if (intendedScene != sceneName || intendedClip != newClip)
                yield break;
        }
        musicSource.volume = 0f;
        musicSource.Stop();

        // Switch and fade in
        musicSource.clip = newClip;
        musicSource.Play();
        time = 0f;
        while (time < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0f, Settings.Instance.MusicVolume, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
            // If scene changed, abort
            if (intendedScene != sceneName || intendedClip != newClip)
                yield break;
        }
        musicSource.volume = Settings.Instance.MusicVolume;
    }

    private IEnumerator FadeOutAndStopSafe(string sceneName)
    {
        float startVolume = musicSource.volume;
        float time = 0f;
        while (time < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
            // If scene changed, abort
            if (intendedScene != sceneName || intendedClip != null)
                yield break;
        }
        musicSource.volume = 0f;
        musicSource.Stop();
        musicSource.clip = null;
    }

    // MusicManager logic
    public void SetMusicMuted(bool muted)
    {
        if (musicSource != null)
            musicSource.mute = muted;
    }

    public bool IsMusicMuted()
    {
        return musicSource != null && musicSource.mute;
    }

    void Update()
    {
        if (Settings.Instance != null)
            musicSource.volume = Settings.Instance.MusicVolume;
    }
}