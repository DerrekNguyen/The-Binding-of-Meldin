using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Sound Manager Script

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private SoundLibrary soundLibrary;
    [SerializeField] private SoundLibrary musicLibrary;

    [SerializeField] static public float globalMusicVolume = 1f; // Between 0-1.0
    private float lastKnownGlobalMusicVolume;
    [SerializeField] static public float globalSoundVolume = 1f; // Between 0-1.0

    static public bool isMusicMuted;
    private float musicVolumeBeforeMute;
    static public bool isSoundMuted;
    
    [SerializeField] public AudioSource backgroundMusic;
    [SerializeField] private float volumeRampDuration = 1f; // For Music

    private string currentSceneName = "";
    private Coroutine volumeRampCoroutine;

    // Singleton Design Pattern
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        lastKnownGlobalMusicVolume = globalMusicVolume;
    }

    // Update
    void Update()
    {
        // If scene change happens handle changing background music
        string gottenSceneName = SceneManager.GetActiveScene().name;

        if(lastKnownGlobalMusicVolume != globalMusicVolume)
        {
            ChangeBackgroundMusic(currentSceneName);
        }

        if (currentSceneName != gottenSceneName)
        {
            currentSceneName = gottenSceneName;    
            ChangeBackgroundMusic(currentSceneName);
        }

        lastKnownGlobalMusicVolume = globalMusicVolume;
    }
    
    private void ChangeBackgroundMusic(string sceneName)
    {
        if (musicLibrary == null || backgroundMusic == null)
        {
            return;
        }
        
        // Get music clip from music library
        AudioClip newMusicClip = musicLibrary.GetClipFromName(sceneName);
        
        if (newMusicClip == null)
        {
            return;
        }
        
        // If the new clip is the same as currently playing return
        if (backgroundMusic.clip == newMusicClip)
        {
            float tv = musicLibrary.GetVolumeFromName(sceneName) * globalMusicVolume;
            backgroundMusic.volume = tv;
            return;
        }
        
        // Stop any existing coroutines
        if (volumeRampCoroutine != null)
        {
            StopCoroutine(volumeRampCoroutine);
        }
        
        // Change the background music
        backgroundMusic.clip = newMusicClip;
        backgroundMusic.loop = true;
        backgroundMusic.volume = 0f;
        backgroundMusic.Play();
        
        // Start coroutine to turn of volume slowly
        float targetVolume = musicLibrary.GetVolumeFromName(sceneName) * globalMusicVolume;
        backgroundMusic.volume = targetVolume;
    }

    public void PlaySound2D(string soundName)
    {
        if(isSoundMuted)
        {
            return;
        }

        if (soundLibrary == null)
        {
            Debug.LogWarning("Sound library is not assigned!");
            return;
        }

        AudioClip clip = soundLibrary.GetClipFromName(soundName);

        if (clip == null)
        {
            Debug.LogWarning($"Sound '{soundName}' not found in sound library!");
            return;
        }
        
        // Create a temporary GameObject with an AudioSource
        GameObject tempAudioObject = new GameObject($"TempAudio_{soundName}");
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
        
        // Configure the AudioSource
        tempAudioSource.clip = clip;
        tempAudioSource.playOnAwake = false;
        tempAudioSource.spatialBlend = 0f;
        tempAudioSource.volume = soundLibrary.GetVolumeFromName(soundName) * globalSoundVolume;
        
        // Play the sound
        tempAudioSource.Play();
        
        // Start coroutine to destroy the object after the clip finishes
        StartCoroutine(DestroyAfterClipFinishes(tempAudioObject, clip.length));
    }
    
    // Destroy sound effect game object after sound is done
    private IEnumerator DestroyAfterClipFinishes(GameObject audioObject, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        if (audioObject != null)
        {
            Destroy(audioObject);
        }
    }

    public void DecreaseMusicVolume()
    {
        if(globalMusicVolume > 0.0f)
            globalMusicVolume -= 0.1f;
        if(globalMusicVolume < 0.0f)
            globalMusicVolume = 0.0f;
    }

    public void IncreaseMusicVolume()
    {
        if(globalMusicVolume < 1.0f)
            globalMusicVolume += 0.1f;
        if(globalMusicVolume > 1.0f)
            globalMusicVolume = 1.0f;
    }

    public void DecreaseSoundVolume()
    {
        if(globalSoundVolume >= 0.1f)
            globalSoundVolume -= 0.1f;
    }

    public void IncreaseSoundVolume()
    {
        if(globalSoundVolume <= 0.9f)
            globalSoundVolume += 0.1f;
    }

    public void MuteMusicVolume()
    {
        if(!isMusicMuted)
        {
            isMusicMuted = true;
            musicVolumeBeforeMute = globalMusicVolume;
            globalMusicVolume = 0;            
        }
        else
        {
            isMusicMuted = false;
            globalMusicVolume = musicVolumeBeforeMute;            
        }
    }  

    public void MuteSoundVolume()
    {
        if(!isSoundMuted)
        {
            isSoundMuted = true;           
        }
        else
        {
            isSoundMuted = false;
        }
    }
}
